using System.IO;

namespace WolfEventCodeCreater.Model
{
	/// <summary>
	/// UserSetting情報から読み込まれた設定関連
	/// </summary>
	public class Config
    {
		private UserSetting userSetting;

		/// <summary>
		/// 対象ウディタのルートパス
		/// </summary>
		public string ProjectRoot;

		/// <summary>
		/// 出力するディレクトリへのフルパス
		/// </summary>
		public string DumpDirPath { get { return RootPathCombine(userSetting.OutputDirName , ""); } }

		/// <summary>
		/// コモンイベントを出力するディレクトリへのフルパス
		/// </summary>
		public string CEvDumpDirPath { get { return Path.Combine(DumpDirPath , "CEv"); } }

		/// <summary>
		/// CDBを出力するディレクトリへのフルパス
		/// </summary>
		public string CDBDumpDirPath { get { return Path.Combine(DumpDirPath , "CDB"); } }

		/// <summary>
		/// UDBを出力するディレクトリへのフルパス
		/// </summary>
		public string UDBDumpDirPath { get { return Path.Combine(DumpDirPath , "UDB"); } }

		/// <summary>
		/// SDBを出力するディレクトリへのフルパス
		/// </summary>
		public string SDBDumpDirPath { get { return Path.Combine(DumpDirPath , "SDB"); } }

		/// <summary>
		/// コモンイベントの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CommonEventPath { get { return RootPathCombine(userSetting.CommonEventPath , "CommonEvent.dat"); } }

		/// <summary>
		/// 可変DBの定義ファイル(project)へのフルパス
		/// </summary>
		public string CDBProjrctFilePath { get { return RootPathCombine(userSetting.CDBProjrctFilePath , "CDataBase.project"); } }

		/// <summary>
		/// 可変DBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CDBDatFilePath { get { return RootPathCombine(userSetting.CDBDatFilePath , "CDataBase.dat"); } }

		/// <summary>
		/// ユーザーDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string UDBProjrctFilePath { get { return RootPathCombine(userSetting.UDBProjrctFilePath , "DataBase.project"); } }

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string UDBDatFilePath { get { return RootPathCombine(userSetting.UDBDatFilePath , "DataBase.dat"); } }

		/// <summary>
		/// システムDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string SDBProjrctFilePath { get { return RootPathCombine(userSetting.SDBProjrctFilePath , "SysDataBase.project"); } }

		/// <summary>
		/// システムDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string SDBDatFilePath { get { return RootPathCombine(userSetting.SDBDatFilePath , "SysDataBase.dat"); } }

		/// <summary>
		/// 出力しないコメントアウト形式へのフルパス
		/// </summary>
		public string CommentOut;

		/// <summary>
		/// 出力のときファイル名にコモン番号をつけるかどうか
		/// </summary>
		public bool IsOutputCommonNumber;



		public Config(UserSetting userSetting)
        {
			this.userSetting = userSetting;

			ProjectRoot = userSetting.ProjectRoot;
			System.Diagnostics.Debug.WriteLine(ProjectRoot , "ProjectRoot");

			CommentOut = userSetting.CommentOut;

			IsOutputCommonNumber = userSetting.IsOutputCommonNumber;
		}

		private string RootPathCombine(string path1, string path2)
		{
			return Path.Combine(ProjectRoot , path1, path2);
		}

		///<summary>ウディタの定義ファイルの存在を確認する</summary>
		public bool IsWoditerDefineFiles(bool isAddAppMes = true)
		{
			bool tmpFlg = true;

			if (!Utils.File.CheckFileExist(CommonEventPath , isAddAppMes ? "コモンイベント定義ファイル" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(CDBProjrctFilePath , isAddAppMes ? "可変DB定義ファイル(project)" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(CDBDatFilePath , isAddAppMes ? "可変DB定義ファイル(dat)" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(UDBProjrctFilePath , isAddAppMes ? "ユーザーDB定義ファイル(project)" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(UDBDatFilePath , isAddAppMes ? "ユーザーDB定義ファイル(dat)" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(SDBProjrctFilePath , isAddAppMes ? "システムDB定義ファイル(project)" : ""))
			{
				tmpFlg = false;
			}
			if (!Utils.File.CheckFileExist(SDBDatFilePath , isAddAppMes ? "システムDB定義ファイル(dat)" : ""))
			{
				tmpFlg = false;
			}

			if (tmpFlg)
			{
				System.Diagnostics.Debug.WriteLine("WoditerDefineFiles are All Exists");
			}
			return tmpFlg;
		}
	}
}
