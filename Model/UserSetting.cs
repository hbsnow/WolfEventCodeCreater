using System.Diagnostics;
using System.Reflection;

namespace WolfEventCodeCreater.Model
{
    public class UserSetting
    {
        public UserSetting()
        {
            AppInfo = new AppInformation();
            WoditorSettings = new WoditorSettingsInfo();
            OutputSettings = new OutputSettingsInfo();
        }

        ///<summary>アプリケーション情報</summary>
        public AppInformation AppInfo;

        public class AppInformation
        {
            ///<summary>アプリ名</summary>
            public string AppName;

            ///<summary>バージョン</summary>
            public string Version;

            public AppInformation()
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                AppName = fileVersionInfo.ProductName;
                Version = fileVersionInfo.FileVersion;
            }
        }

        ///<summary>Woditor関連設定</summary>
        public WoditorSettingsInfo WoditorSettings;

        public class WoditorSettingsInfo
        {
            /// <summary>ルートパス</summary>
            public string ProjectRoot = "";

            /// <summary>コモンイベントの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string CommonEventDir = @"Data\BasicData";

            /// <summary>コモンイベントの定義ファイル名(dat)</summary>
            public string CommonEventFileName = @"CommonEvent.dat";

            /// <summary>可変DBの定義ファイル(project)が格納されたディレクトリ</summary>
            public string CDBProjrctFileDir = @"Data\BasicData";

            /// <summary>可変DBの定義ファイル名(project)</summary>
            public string CDBProjrctFileFileName = @"CDataBase.project";

            /// <summary>可変DBの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string CDBDatFileDir = @"Data\BasicData";

            /// <summary>可変DBの定義ファイル名(dat)</summary>
            public string CDBDatFileFileName = @"CDataBase.dat";

            /// <summary>ユーザーDBの定義ファイル(project)が格納されたディレクトリ</summary>
            public string UDBProjrctFileDir = @"Data\BasicData";

            /// <summary>ユーザーDBの定義ファイル名(project)</summary>
            public string UDBProjrctFileName = @"DataBase.project";

            /// <summary>ユーザーDBの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string UDBDatFileDir = @"Data\BasicData";

            /// <summary>ユーザーDBの定義ファイル名(dat)</summary>
            public string UDBDatFileName = @"DataBase.dat";

            /// <summary>システムDBの定義ファイル(project)が格納されたディレクトリ</summary>
            public string SDBProjrctFileDir = @"Data\BasicData";

            /// <summary>システムDBの定義ファイル名(project)</summary>
            public string SDBProjrctFileName = @"SysDataBase.project";

            /// <summary>システムDBの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string SDBDatFileDir = @"Data\BasicData";

            /// <summary>システムDBの定義ファイル名(dat)</summary>
            public string SDBDatFileName = @"SysDataBase.dat";

            /// <summary>マップの定義ファイル(mps)が格納されたディレクトリ</summary>
            public string MapDataDir = @"Data";

            /// <summary>マップツリーの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string MapTreeDir = @"Data\BasicData";

            /// <summary>マップツリーの定義ファイル名(dat)</summary>
            public string MapTreeFileName = @"MapTree.dat";

            /// <summary>マップツリー展開状態の定義ファイル(dat)が格納されたディレクトリ</summary>
            public string MapTreeOpenStatusDir = @"Data\BasicData";

            /// <summary>マップツリー展開状態の定義ファイル名(dat)</summary>
            public string MapTreeOpenStatusFileName = @"MapTreeOpenStatus.dat";

            /// <summary>タイルセットの定義ファイル(dat)が格納されたディレクトリ</summary>
            public string TileSetDataDir = @"Data\BasicData";

            /// <summary>タイルセットの定義ファイル名(dat)</summary>
            public string TileSetDataFileName = @"TileSetData.dat";
        }

        /// <summary>出力関連設定</summary>
        public OutputSettingsInfo OutputSettings;

        public class OutputSettingsInfo
        {
            /// <summary>出力処理実行時の日時</summary>
            public string DateTime = "";

            /// <summary>出力するディレクトリ名(ルート直下)</summary>
            public string OutputDirName = "Dump";

            /// <summary>指定された出力ディレクトリパス(ルート外ディレクトリも指定可能であり、OutputDirNameよりも優先される)</summary>
            public string SpecifiedOutputDirPath = "";

            /// <summary>出力フォルダに出力日時の接尾辞をつけるかどうか</summary>
            public bool IsAdditionalDateTimeToOutputDirNameSuffiix = false;

            /// <summary>出力しないコモンのコメントアウト形式</summary>
            public string CommentOut = "//";

            /// <summary>出力のときファイル名にコモン番号をつけるかどうか</summary>
            public bool IsOutputCommonNumber = false;
        }

        ///<summary>settings.xmlを上書きし、新たなConfigインスタンスを生成する</summary>
        public Config OverWriteUserSettingFile(string projectRoot, string dateTime)
        {
            WoditorSettings.ProjectRoot = projectRoot;
            OutputSettings.DateTime = dateTime;

            Utils.File.WriteUserSetting(this);

            Config config = new Model.Config(this);

            return config;
        }
    }
}