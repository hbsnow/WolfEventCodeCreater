using System.Diagnostics;
using System.Reflection;

namespace WolfEventCodeCreater.Model
{
	public class UserSetting
	{
		public UserSetting()
		{
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			AppName = fileVersionInfo.ProductName;
			Version = fileVersionInfo.FileVersion;
		}

		///<summary>アプリ名</summary>
		public string AppName { get; set; }

		///<summary>バージョン</summary>
		public string Version { get; set; }
		
		/// <summary>
		/// ルートパス
		/// </summary>
		public string ProjectRoot = "";

		/// <summary>
		/// 出力処理実行時の日時
		/// </summary>
		public string DateTime = "";

		/// <summary>
		/// 出力するディレクトリ名
		/// </summary>
		public string OutputDirName = "Dump";

		/// <summary>
		/// コモンイベントの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string CommonEventDir = @"Data\BasicData";

		/// <summary>
		/// 可変DBの定義ファイル(project)が格納されたディレクトリ
		/// </summary>
		public string CDBProjrctFileDir = @"Data\BasicData";

		/// <summary>
		/// 可変DBの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string CDBDatFileDir = @"Data\BasicData";

		/// <summary>
		/// ユーザーDBの定義ファイル(project)が格納されたディレクトリ
		/// </summary>
		public string UDBProjrctFileDir = @"Data\BasicData";

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string UDBDatFileDir = @"Data\BasicData";

		/// <summary>
		/// システムDBの定義ファイル(project)が格納されたディレクトリ
		/// </summary>
		public string SDBProjrctFileDir = @"Data\BasicData";

		/// <summary>
		/// システムDBの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string SDBDatFileDir = @"Data\BasicData";

		/// <summary>
		///マップの定義ファイル(map)が格納されたディレクトリ
		/// </summary>
		public string MapDataDir = @"Data\MapData";

		/// <summary>
		///マップツリーの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string MapTreeDir = @"Data\BasicData";

		/// <summary>
		///マップツリー展開状態の定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string MapTreeOpenStatusDir = @"Data\BasicData";

		/// <summary>
		///タイルセットの定義ファイル(dat)が格納されたディレクトリ
		/// </summary>
		public string TileSetDataDir = @"Data\BasicData";

		/// <summary>
		/// 出力しないコモンのコメントアウト形式
		/// </summary>
		/// 
		public string CommentOut = "//";

		/// <summary>
		/// 出力フォルダに出力日時の接尾辞をつけるかどうか
		/// </summary>
		public bool IsAdditionalDateTimeToOutputDirNameSuffiix = false;

		/// <summary>
		/// 出力のときファイル名にコモン番号をつけるかどうか
		/// </summary>
		public bool IsOutputCommonNumber = false;

		///<summary>settings.xmlを上書きし、新たなConfigインスタンスを生成する</summary>
		public Config OverWriteUserSettingFile(string projectRoot, string dateTime)
		{
			ProjectRoot = projectRoot;
			DateTime = dateTime;

			Utils.File.WriteUserSetting(this);

			Config config = new Model.Config(this);

			return config;
		}
    }
}
