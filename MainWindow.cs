using System;
using System.IO;
using System.Windows.Forms;

namespace WolfEventCodeCreater
{
    public partial class MainWindow : Form
    {
        public CodeCreater CodeCreater;

        public MainWindow(CodeCreater Creater)
        {
            InitializeComponent();
            CodeCreater = Creater;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを選択してください。";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                CodeCreater.Rootpath = fbd.SelectedPath;
                textBox1.Text = fbd.SelectedPath;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string message = CodeCreater.Create();
                
                textBox2.Text = textBox2.Text == "" ? message : message + "\r\n" + textBox2.Text;

            }
            catch(Exception err)
            {
                textBox2.Text = err.ToString();
            }
        }
    }
}
