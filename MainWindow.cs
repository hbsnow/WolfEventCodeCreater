using System;
using System.IO;
using System.Windows.Forms;
using WodiKs.IO;

namespace WolfEventCodeCreater
{
    public partial class MainWindow : Form
    {
		private Model.UserSetting userSetting;
		private Model.Config Config;

        public MainWindow()
        {
            InitializeComponent();
			userSetting = Utils.File.LoadUserSetting();
			Config = new Model.Config(userSetting);

			textBox1.Text = userSetting.ProjectRoot;
		}



        private void selectProject(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.Description = "プロジェクトのルートディレクトリを選択してください。";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
				textBox1.Text = fbd.SelectedPath;
				if (!button2.Enabled)
				{
					textBox2.Text = "ウディタ定義ファイルが見つかりません。" + "\r\n" + textBox2.Text;
				}
            }
        }


        
        private void create(object sender, EventArgs e)
        {
			string now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			string message = $"------出力実行({ now })------" + "\r\n";
			System.Diagnostics.Debug.WriteLine(message);

			try
            {
				userSetting.ProjectRoot = Config.ProjectRoot;
				Utils.File.WriteUserSetting(userSetting);

				var CommonEventReader = new CommonEventDatReader(Config.CommonEventPath);

                var CodeCreater = new CodeCreater(Config, CommonEventReader);

                var writeMessage = CodeCreater.Write();

				message = message + writeMessage + "\r\n" + "--------------------------------" + "\r\n";

				textBox2.Text = message + textBox2.Text;

            }
            catch(Exception err)
            {
                textBox2.Text = message+ err.ToString() + "--------------------------------" + "\r\n" + textBox2.Text;
            }
        }



        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                $"Version { Application.ProductVersion }",
                "バージョン情報",
                MessageBoxButtons.OK,
                MessageBoxIcon.None
            );
        }



        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void setting(object sender, EventArgs e)
        {
            var settingWindow = new SettingWindow();
            settingWindow.ShowDialog(this);
            settingWindow.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			Config.ProjectRoot = textBox1.Text;
			Config.PathChangeWithRootChanged(userSetting);
			button2.Enabled = Config.IsWoditerDefineFiles();
        }
	}
}
