using System;
using System.Windows.Forms;

namespace WolfEventCodeCreater
{
    public partial class SettingWindow : Form
    {
        public SettingWindow()
        {
            InitializeComponent();

            var userSetting = Utils.File.LoadUserSetting();

            textBox1.Text = userSetting.OutputDirName;
            textBox2.Text = userSetting.CommentOut;
            comboBox1.Items.Add("出力する");
            comboBox1.Items.Add("出力しない");
            comboBox1.SelectedIndex = userSetting.IsOutputCommonNumber ? 0 : 1;
        }



        private void submit(object sender, EventArgs e)
        {
            var userSetting = new Model.UserSetting();
            userSetting.OutputDirName = textBox1.Text;
            userSetting.CommentOut = textBox2.Text;
            userSetting.IsOutputCommonNumber = comboBox1.SelectedIndex == 0 ? true : false;

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
