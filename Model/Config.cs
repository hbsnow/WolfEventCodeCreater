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
		/// 出力しないコメントアウト形式へのフルパス
		/// </summary>
		public string CommentOut;

		/// <summary>
		/// 出力のときファイル名にコモン番号をつけるかどうか
		/// </summary>
		public bool IsOutputCommonNumber;



		public Config(UserSetting userSetting)
        {
			ProjectRoot = userSetting.ProjectRoot;
			System.Diagnostics.Debug.WriteLine(ProjectRoot , "ProjectRoot");
			PathChangeWithRootChanged(userSetting);

			CommentOut = userSetting.CommentOut;

			IsOutputCommonNumber = userSetting.IsOutputCommonNumber;
		}

		private string RootPathCombine(string path1, string path2)
		{
			return Path.Combine(ProjectRoot , path1, path2);
		}

		///<summary>ルートパスに依存するパスの書き換え</summary>
		public void PathChangeWithRootChanged(UserSetting userSetting) { 
			DumpDirPath = RootPathCombine(userSetting.OutputDirName,"");
			CEvDumpDirPath = Path.Combine(DumpDirPath, "CEv");
			CDBDumpDirPath = Path.Combine(DumpDirPath, "CDB");
			UDBDumpDirPath = Path.Combine(DumpDirPath, "UDB");
			SDBDumpDirPath = Path.Combine(DumpDirPath, "SDB");
			CommonEventPath = RootPathCombine(userSetting.CommonEventPath, "CommonEvent.dat");
			CDBProjrctFilePath = RootPathCombine(userSetting.CDBProjrctFilePath, "CDataBase.project");
			CDBDatFilePath = RootPathCombine(userSetting.CDBDatFilePath , "CDataBase.dat");
			UDBProjrctFilePath = RootPathCombine(userSetting.UDBProjrctFilePath , "DataBase.project");
			UDBDatFilePath = RootPathCombine(userSetting.UDBDatFilePath , "DataBase.dat");
			SDBProjrctFilePath = RootPathCombine(userSetting.SDBProjrctFilePath , "SysDataBase.project");
			SDBDatFilePath = RootPathCombine(userSetting.SDBDatFilePath , "SysDataBase.dat");
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
			return tmpFlg;
		}
	}
}
