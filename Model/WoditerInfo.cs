using System.Collections.Generic;
using WodiKs.DB;
using WodiKs.Ev.Common;
using WodiKs.IO;
using WodiKs.Map;
using WodiKs.Map.Tile;

namespace WolfEventCodeCreater.Model
{
    public class WoditerInfo
    {
        public enum WoditerInfoCategory
        {
            CEv, CDB, UDB, SDB, Map, MapTree, TileSet
        }

        public CommonEventManager CEvMgr { get; private set; }
        public Database CDB { get; private set; }
        public Database UDB { get; private set; }
        public Database SDB { get; private set; }
        public Dictionary<string, MapData> MapDataList { get; private set; }
        public MapTree MapTree { get; private set; }
        public TileSetManager TileMgr { get; private set; }
        public Config Config { get; private set; }

        public WoditerInfo(Config config)
        {
            CEvMgr = null;
            CDB = null;
            UDB = null;
            SDB = null;
            MapDataList = null;
            MapTree = null;
            TileMgr = null;
            this.Config = config;

            CEvMgr = CommonEventRead();
            CDB = DatabaseRead(WoditerInfoCategory.CDB);
            UDB = DatabaseRead(WoditerInfoCategory.UDB);
            SDB = DatabaseRead(WoditerInfoCategory.SDB);
            MapDataList = MapDataListRead();
            MapTree = MapTreeRead();
            TileMgr = TileSetRead();
        }

        /// <summary>読込結果を判定する</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checkObject">判定対象</param>
        /// <param name="woditerInfoCategory"></param>
        /// <returns>判定結果(true:読込成功, false:読込失敗)</returns>
        private bool CheckReadResult<T>(T checkObject, WoditerInfoCategory woditerInfoCategory) where T: class
        {
            if(checkObject == null)
            {
                // アプリメッセージに読込失敗結果を追加
                AppMesOpp.AddAppMessge($"{woditerInfoCategory.ToString()} の情報取得に失敗しました。", true);
                return false;
            }
            return true;
        }

        ///<summary>コモンイベントを読込</summary>
        private CommonEventManager CommonEventRead()
        {
            CommonEventManager commonEventManager = null;

            // 定義ファイルの存在チェック
            if (Utils.File.CheckFileExist(Config.CommonEventPath, $"コモンイベントの定義ファイル"))
            {
                var commonEventDatReader = new CommonEventDatReader();
                commonEventManager = commonEventDatReader.ReadFile(Config.CommonEventPath);
            }

            // 読込結果判定処理
            CheckReadResult(commonEventManager, WoditerInfoCategory.CEv);

            return commonEventManager;
        }

        ///<summary>DBを読込</summary>
        ///<param name="db">読込対象のDB</param>
        private Database DatabaseRead(WoditerInfoCategory woditerInfoCategory_DB)
        {
            Database database = null;
            string projectFilePath = "";
            string datFilePath = "";
            string dbName = "";
            bool isProjectFileExist = false;
            bool isDatFileExist = false;

            switch (woditerInfoCategory_DB)
            {
                case WoditerInfoCategory.CDB:
                    {
                        projectFilePath = Config.CDBProjrctFilePath;
                        datFilePath = Config.CDBDatFilePath;
                        dbName = "可変データベース";
                        break;
                    }
                case WoditerInfoCategory.UDB:
                    {
                        projectFilePath = Config.UDBProjrctFilePath;
                        datFilePath = Config.UDBDatFilePath;
                        dbName = "ユーザーデータベース";
                        break;
                    }
                case WoditerInfoCategory.SDB:
                    {
                        projectFilePath = Config.SDBProjrctFilePath;
                        datFilePath = Config.SDBDatFilePath;
                        dbName = "システムデータベース";
                        break;
                    }
                default:
                    {
                        // 念のため
                        break;
                    }
            }

            // 定義ファイルの存在チェック
            isProjectFileExist = Utils.File.CheckFileExist(projectFilePath, $"{ dbName }の定義ファイル(.project)");
            isDatFileExist = Utils.File.CheckFileExist(datFilePath, $"{ dbName }の定義ファイル(.dat)");

            if (isProjectFileExist && isDatFileExist)
            {
                DatabaseFileReader dfr = new DatabaseFileReader(projectFilePath, datFilePath);
                database = dfr.GetReadData();
            }

            // 読込結果判定処理
            CheckReadResult(database, woditerInfoCategory_DB);

            return database;
        }

        ///<summary>パラメータのdatabaseCategoryごとに対応するWoditerInfoクラスのDatabase型プロパティを返す</summary>
        public Database GetDatabaseSource(Database.DatabaseCategory databaseCategory)
        {
            switch (databaseCategory)
            {
                case Database.DatabaseCategory.Changeable:
                    {
                        return CDB;
                    }
                case Database.DatabaseCategory.User:
                    {
                        return UDB;
                    }
                case Database.DatabaseCategory.System:
                    {
                        return SDB;
                    }
                case Database.DatabaseCategory.CommonEvent:
                    {
                        System.Diagnostics.Debug.WriteLine("WoditerInfo.GetDatabaseSource()のNULLエラー");
                        return null;
                    }
                default:
                    return null;
            }
        }

        ///<summary>マップデータを読込</summary>
        private Dictionary<string, MapData> MapDataListRead()
        {
            Dictionary<string, MapData> mapDataList = new Dictionary<string, MapData>();

            // 定義ファイル格納フォルダの存在チェック
            if (Utils.File.CheckDirectoryExist(Config.MapDataDir, "", false))
            {
                List<string> mapFileList = Utils.File.GetFilesInDirectory(Config.MapDataDir, "*.mps", true);

                foreach (var mapFile in mapFileList)
                {
                    var mdr = new MapDataReader(mapFile);
                    MapData mapData = mdr.GetReadData();

                    // 読込結果判定処理
                    if(CheckReadResult(mapData, WoditerInfoCategory.Map))
                    {
                        mapDataList.Add(mapFile, mapData);
                    }
                }
            }

            return mapDataList;
        }

        ///<summary>マップツリーを読込</summary>
        private MapTree MapTreeRead()
        {
            MapTree mapTree = null;

            // 定義ファイルの存在チェック
            if (Utils.File.CheckFileExist(Config.MapTreeFilePath, $"マップツリーの定義ファイル")
                & Utils.File.CheckFileExist(Config.MapTreeOpenStatusFilePath, $"マップツリー展開状態の定義ファイル"))
            {
                var mapTreeReader = new MapTreeReader(Config.MapTreeFilePath, Config.MapTreeOpenStatusFilePath);
                mapTree = mapTreeReader.GetReadData();
            }

            // 読込結果判定処理
            //! WodiKs.dll ver0.75にMapTree読込エラーが有り
            CheckReadResult(mapTree, WoditerInfoCategory.MapTree);

            return mapTree;
        }

        ///<summary>タイルセットを読込</summary>
        private TileSetManager TileSetRead()
        {
            TileSetManager tileSetManager = null;

            // 定義ファイルの存在チェック
            if (Utils.File.CheckFileExist(Config.TileSetDataFilePath, $"タイルセットの定義ファイル"))
            {
                var tileSetDataReader = new TileSetDataReader(Config.TileSetDataFilePath);
                tileSetManager = tileSetDataReader.GetReadData();
            }

            // 読込結果判定処理
            CheckReadResult(tileSetManager, WoditerInfoCategory.TileSet);

            return tileSetManager;
        }
    }
}