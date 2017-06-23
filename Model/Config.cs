using System.IO;

namespace WolfEventCodeCreater.Model
{
    /// <summary>
    /// 設定関連
    /// </summary>
    public class Config
    {
        public string ProjectRoot;
        public string DumpDir;
        public string CommonEventPath;



        public Config(string root)
        {
            SetPath(root);
        }

        
        
        /// <summary>
        /// パスの一括設定
        /// </summary>
        /// <param name="value"></param>
        private void SetPath(string value)
        {
            ProjectRoot = value;
            DumpDir = Path.Combine(value, "Dump");
            CommonEventPath = Path.Combine(value, @"Data\BasicData\CommonEvent.dat");
        }
    }
}
