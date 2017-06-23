using System;
using System.IO;
using System.Windows.Forms;
using WodiKs.IO;

namespace WolfEventCodeCreater
{
    public partial class MainWindow : Form
    {
        private Model.Config Config;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void selectProject(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.Description = "プロジェクトのルートディレクトリを選択してください。";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Config = new Model.Config(fbd.SelectedPath);

                textBox1.Text = fbd.SelectedPath;
                button2.Enabled = true;
            }
        }


        
        private void create(object sender, EventArgs e)
        {
            try
            {
                var CommonEventReader = new CommonEventDatReader(Config.CommonEventPath);


                var CodeCreater = new CodeCreater(Config, CommonEventReader);

                string message = CodeCreater.Write();
                
                textBox2.Text = textBox2.Text == "" ? message : message + "\r\n" + textBox2.Text;

            }
            catch(Exception err)
            {
                textBox2.Text = err.ToString();
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
    }
}
