using System;
using System.Windows.Forms;
using WodiKs.IO;

namespace WolfEventCodeCreater
{
    public partial class MainWindow : Form
    {
        private Config Config;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを選択してください。";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Config = new Config(fbd.SelectedPath);
                textBox1.Text = fbd.SelectedPath;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
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
    }
}
