﻿using System;
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
			AppMesOpp.ClearAppMessge();

			string now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			string headMessage = ($"------出力実行({ now })------");
			AppMesOpp.AddAppMessge(headMessage);
			System.Diagnostics.Debug.WriteLine(headMessage);

			try
            {
				userSetting.ProjectRoot = Config.ProjectRoot;
				// settings.xmlの上書き
				Utils.File.WriteUserSetting(userSetting);

				Config = new Model.Config(userSetting);

				var CommonEventReader = new CommonEventDatReader();

                var CodeCreater = new CodeCreater(Config);

				// 出力処理
				CodeCreater.Write();
            }
            catch(Exception err)
            {
				AppMesOpp.AddAppMessge(err.ToString());
            }

			// settings.xmlを出力先ディレクトリに出力
			Utils.File.WriteUserSetting(userSetting, Config.DumpDirPath, $"_{ now }");

			// メッセージの表示
			AppMesOpp.AddSeparatorAppMessge();
			textBox2.Text = AppMesOpp.ReturnAppMessge(true) + textBox2.Text;
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
            var settingWindow = new SettingWindow(userSetting);
            settingWindow.ShowDialog(this);
            settingWindow.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			Config.ProjectRoot = textBox1.Text;
			button2.Enabled = Config.IsWoditerDefineFiles();
        }
	}
}
