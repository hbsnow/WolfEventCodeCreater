using System.IO;

namespace WolfEventCodeCreater.Model
{
	/// <summary>
	/// UserSetting情報から読み込まれた設定関連
	/// </summary>
	public class Config
    {
		private UserSetting userSetting;
		private UserSetting.WoditorSettingsInfo woditorSettings;
		private UserSetting.OutputSettingsInfo outputSettings;

		/// <summary>
		/// 対象ウディタのルートパス
		/// </summary>
		public string ProjectRoot;

		/// <summary>
		/// 出力処理実行時の日時
		/// </summary>
		public string DateTime;

		#region 出力ディレクトリ
		/// <summary>
		/// 出力するディレクトリへのフルパス
		/// </summary>
		public string DumpDirPath { get {
			string filenameSuffiix = outputSettings.IsAdditionalDateTimeToOutputDirNameSuffiix ? $"_{DateTime}" : "";
			string dumpDir = "";

			if(outputSettings.SpecifiedOutputDirPath == "")
			{
				dumpDir = RootPathCombine(outputSettings.OutputDirName, filenameSuffiix);
			}
			else
			{
				dumpDir = Path.Combine(outputSettings.SpecifiedOutputDirPath, outputSettings.OutputDirName);
				Utils.File.CheckDirectoryExist(dumpDir, "出力先パス", true);
			}

			return dumpDir + filenameSuffiix;
		} }

		/// <summary>
		/// コモンイベントを出力するディレクトリへのフルパス
		/// </summary>
		public string CEvDumpDirPath { get { return Path.Combine(DumpDirPath , @"CEv"); } }

		/// <summary>
		/// CDBを出力するディレクトリへのフルパス
		/// </summary>
		public string CDBDumpDirPath { get { return Path.Combine(DumpDirPath , @"CDB"); } }

		/// <summary>
		/// UDBを出力するディレクトリへのフルパス
		/// </summary>
		public string UDBDumpDirPath { get { return Path.Combine(DumpDirPath , @"UDB"); } }

		/// <summary>
		/// SDBを出力するディレクトリへのフルパス
		/// </summary>
		public string SDBDumpDirPath { get { return Path.Combine(DumpDirPath , @"SDB"); } }

		/// <summary>
		/// Mapを出力するディレクトリへのフルパス
		/// </summary>
		public string MapDumpDirPath { get { return Path.Combine(DumpDirPath , @"Map\Map"); } }

		/// <summary>
		/// MapTreeを出力するディレクトリへのフルパス
		/// </summary>
		public string MapTreeDumpDirPath { get { return Path.Combine(DumpDirPath, @"Map"); } }

		/// <summary>
		/// TileSetを出力するディレクトリへのフルパス
		/// </summary>
		public string TileSetDumpDirPath { get { return Path.Combine(DumpDirPath, @"TileSet"); } }
	#endregion

		#region ウディタ定義ファイル
		/// <summary>
		/// コモンイベントの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CommonEventPath { get { return RootPathCombine(woditorSettings.CommonEventDir, woditorSettings.CommonEventFileName); } }

		/// <summary>
		/// 可変DBの定義ファイル(project)へのフルパス
		/// </summary>
		public string CDBProjrctFilePath { get { return RootPathCombine(woditorSettings.CDBProjrctFileDir , woditorSettings.CDBProjrctFileFileName); } }

		/// <summary>
		/// 可変DBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string CDBDatFilePath { get { return RootPathCombine(woditorSettings.CDBDatFileDir , woditorSettings.CDBDatFileFileName); } }

		/// <summary>
		/// ユーザーDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string UDBProjrctFilePath { get { return RootPathCombine(woditorSettings.UDBProjrctFileDir , woditorSettings.UDBProjrctFileName); } }

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string UDBDatFilePath { get { return RootPathCombine(woditorSettings.UDBDatFileDir , woditorSettings.UDBDatFileName); } }

		/// <summary>
		/// システムDBの定義ファイル(project)へのフルパス
		/// </summary>
		public string SDBProjrctFilePath { get { return RootPathCombine(woditorSettings.SDBProjrctFileDir , woditorSettings.SDBProjrctFileName); } }

		/// <summary>
		/// システムDBの定義ファイル(dat)へのフルパス
		/// </summary>
		public string SDBDatFilePath { get { return RootPathCombine(woditorSettings.SDBDatFileDir , woditorSettings.SDBDatFileName); } }

		/// <summary>
		/// マップの定義ファイル(map)が格納されたディレクトリのフルパス
		/// </summary>
		public string MapDataDir { get { return RootPathCombine(woditorSettings.MapDataDir , ""); } }

		/// <summary>
		///マップツリーの定義ファイル(dat)へのフルパス
		/// </summary>
		public string MapTreeFilePath { get { return RootPathCombine(woditorSettings.MapTreeDir , woditorSettings.MapTreeFileName); } }

		/// <summary>
		///マップツリー展開状態の定義ファイル(dat)へのフルパス
		/// </summary>
		public string MapTreeOpenStatusFilePath { get { return RootPathCombine(woditorSettings.MapTreeOpenStatusDir , woditorSettings.MapTreeOpenStatusFileName); } }

		/// <summary>
		///タイルセットの定義ファイル(dat)へのフルパス
		/// </summary>
		public string TileSetDataFilePath { get { return RootPathCombine(woditorSettings.TileSetDataDir , woditorSettings.TileSetDataFileName); } }
		#endregion

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
			woditorSettings = userSetting.WoditorSettings;
			outputSettings = userSetting.OutputSettings;

			ProjectRoot = woditorSettings.ProjectRoot;
			System.Diagnostics.Debug.WriteLine(ProjectRoot , "ProjectRoot");

			DateTime = outputSettings.DateTime;
			CommentOut = outputSettings.CommentOut;
			IsOutputCommonNumber = outputSettings.IsOutputCommonNumber;
		}

		private string RootPathCombine(string path1, string path2)
		{
			return Path.Combine(ProjectRoot , path1, path2);
		}

		///<summary>ウディタの定義ファイルの存在を確認する</summary>
		public bool IsWoditerDefineFiles(bool isAddAppMes = true)
		{
			bool tmpFlg = true;

			tmpFlg = IsCDBFileExist(isAddAppMes) & IsCDBFileExist(isAddAppMes) & IsUDBFileExist(isAddAppMes) & IsSDBFileExist(isAddAppMes)
				& IsMapFileExist(isAddAppMes) & IsTileFileExist(isAddAppMes);

			if (tmpFlg)
			{
				System.Diagnostics.Debug.WriteLine("WoditerDefineFiles are All Exists");
			}
			return tmpFlg;
		}

		private bool IsCEvFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckFileExist(CommonEventPath , isAddAppMes ? "コモンイベント定義ファイル" : "");
		}

		private bool IsCDBFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckFileExist(CDBProjrctFilePath , isAddAppMes ? "可変DB定義ファイル(project)" : "")
				& Utils.File.CheckFileExist(CDBDatFilePath , isAddAppMes ? "可変DB定義ファイル(dat)" : "");
		}

		private bool IsUDBFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckFileExist(UDBProjrctFilePath , isAddAppMes ? "ユーザーDB定義ファイル(project)" : "")
				& Utils.File.CheckFileExist(UDBDatFilePath , isAddAppMes ? "ユーザーDB定義ファイル(dat)" : "");
		}

		private bool IsSDBFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckFileExist(SDBProjrctFilePath , isAddAppMes ? "システムDB定義ファイル(project)" : "")
				& Utils.File.CheckFileExist(SDBDatFilePath , isAddAppMes ? "システムDB定義ファイル(dat)" : "");
		}

		private bool IsMapFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckDirectoryExist(MapDataDir , isAddAppMes ? "マップデータ格納フォルダ" : "" , false)
				& Utils.File.CheckFileExist(MapTreeFilePath , isAddAppMes ? "マップツリーファイル(dat)" : "")
				& Utils.File.CheckFileExist(MapTreeOpenStatusFilePath , isAddAppMes ? "マップツリー展開状態ファイル(dat)" : "");
		}

		private bool IsTileFileExist(bool isAddAppMes)
		{
			return Utils.File.CheckFileExist(TileSetDataFilePath , isAddAppMes ? "タイルセット定義ファイル" : "");
		}

		///<summary>出力先ディレクトリの取得</summary>
		public string GetOutputDir(WoditerInfo.WoditerInfoCategory woditerInfoCategory)
		{
			switch (woditerInfoCategory)
			{
				case WoditerInfo.WoditerInfoCategory.CEv:
					{
						return CEvDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.CDB:
					{
						return CDBDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.UDB:
					{
						return UDBDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.SDB:
					{
						return SDBDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.Map:
					{
						return MapDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.MapTree:
					{
						return MapTreeDumpDirPath;
					}
				case WoditerInfo.WoditerInfoCategory.TileSet:
					{
						return TileSetDumpDirPath;
					}
				default:
					return "";
			}
		}
	}
}
