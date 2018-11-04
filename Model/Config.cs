using System.IO;

namespace WolfEventCodeCreater.Model
{
	/// <summary>
	/// UserSetting情報から読み込まれた設定関連
	/// </summary>
	public class Config
    {
        public string ProjectRoot;
        public string DumpDir;
        public string CommonEventPath;
		public string CommentOut_Common;



        public Config(string root, UserSetting userSetting)
        {
            ProjectRoot = root;
            DumpDir = Path.Combine(root, Utils.String.FormatFilename(userSetting.OutputDirName));
            CommonEventPath = Path.Combine(root, @"Data\BasicData\CommonEvent.dat");
			CommentOut_Common = userSetting.CommentOut;

		}
        
    }
}
