using System;
using System.Windows.Forms;
using WolfEventCodeCreater.Model;

namespace WolfEventCodeCreater
{
    public partial class SettingWindow : Form
    {
        private UserSetting userSetting;
        private UserSetting.WoditorSettingsInfo woditorSettings;
        private UserSetting.OutputSettingsInfo outputSettings;

        public SettingWindow(UserSetting userSetting)
        {
            InitializeComponent();

            this.userSetting = userSetting;
            woditorSettings = userSetting.WoditorSettings;
            outputSettings = userSetting.OutputSettings;

            textBox1.Text = outputSettings.OutputDirName;
            textBox2.Text = outputSettings.CommentOut;
            comboBox1.Items.Add("出力する");
            comboBox1.Items.Add("出力しない");
            comboBox1.SelectedIndex = outputSettings.IsOutputCommonNumber ? 0 : 1;
        }



        private void submit(object sender, EventArgs e)
        {
            outputSettings.OutputDirName = textBox1.Text;
            outputSettings.CommentOut = textBox2.Text;
            outputSettings.IsOutputCommonNumber = comboBox1.SelectedIndex == 0 ? true : false;

            Utils.File.WriteUserSetting(userSetting);

            var result = MessageBox.Show(
                $"設定を変更しました。",
                "確認",
                MessageBoxButtons.OK,
                MessageBoxIcon.None
            );

            Close();
        }



        private void cancel(object sender, EventArgs e)
        {
            Close();
        }

    }
}
