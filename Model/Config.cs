using System.IO;

namespace WolfEventCodeCreater.Model
{
	/// <summary>
	/// UserSetting情報から読み込まれた設定関連
	/// </summary>
	public class Config
    {
		/// <summary>
		/// 対象ウディタのルートパス
		/// </summary>
		public string ProjectRoot;

		/// <summary>
		/// 出力するディレクトリへのフルパス
		/// </summary>
		public string DumpDirPath;

		/// <summary>
		/// コモンイベントを出力するディレクトリへのフルパス
		/// </summary>
		public string CEvDumpDirPath;

		/// <summary>
		/// CDBを出力するディレクトリへのフルパス
		/// </summary>
		public string CDBDumpDirPath;

		/// <summary>
		/// UDBを出力するディレクトリへのフルパス
		/// </summary>
		public string UDBDumpDirPath;

		/// <summary>
		/// SDBを出力するディレクトリへのフルパス
		/// </summary>
		public string SDBDumpDirPath;

		/// <summary>
		/// コモンイベントの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CommonEventPath;

		/// <summary>
		/// 可変DBの定義ファイル(project)へのフルパス
		/// </summary>
		public string CDBProjrctFilePath;

		/// <summary>
		/// 可変DBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CDBDatFilePath;

		/// <summary>
		/// ユーザーDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string UDBProjrctFilePath;

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string UDBDatFilePath;

		/// <summary>
		/// システムDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string SDBProjrctFilePath;

		/// <summary>
		/// システムDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string SDBDatFilePath;

		/// <summary>
		/// 出力しないコモンのコメントアウト形式へのフルパス
		/// </summary>
		public string CommentOut_Common;

		/// <summary>
		/// 出力のときファイル名にコモン番号をつけるかどうか
		/// </summary>
		public bool IsOutputCommonNumber;



		public Config(string root, UserSetting userSetting)
        {
			ProjectRoot =root;
			System.Diagnostics.Debug.WriteLine(ProjectRoot , "ProjectRoot");
			DumpDirPath = RootPathCombine(userSetting.OutputDirName);
			CEvDumpDirPath = Path.Combine(DumpDirPath , "CEv");
			CDBDumpDirPath = Path.Combine(DumpDirPath , "CDB");
			UDBDumpDirPath = Path.Combine(DumpDirPath , "UDB");
			SDBDumpDirPath = Path.Combine(DumpDirPath , "SDB");
			CommonEventPath = RootPathCombine(userSetting.CommonEventPath);
			CDBProjrctFilePath = RootPathCombine(userSetting.CDBProjrctFilePath);
			CDBDatFilePath = RootPathCombine(userSetting.CDBDatFilePath);
			UDBProjrctFilePath = RootPathCombine(userSetting.UDBProjrctFilePath);
			UDBDatFilePath = RootPathCombine(userSetting.UDBDatFilePath);
			SDBProjrctFilePath = RootPathCombine(userSetting.SDBProjrctFilePath);
			SDBDatFilePath = RootPathCombine(userSetting.SDBDatFilePath);

			CommentOut_Common = userSetting.CommentOut;

			IsOutputCommonNumber = userSetting.IsOutputCommonNumber;
		}
		private string RootPathCombine(string path)
		{
			return Path.Combine(ProjectRoot , path);
		}
	}
}
