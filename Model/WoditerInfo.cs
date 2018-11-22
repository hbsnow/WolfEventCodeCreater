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
		public CommonEventManager CEvMgr { get; private set; }
		public Database CDB { get; private set; }
		public Database UDB { get; private set; }
		public Database SDB { get; private set; }
		public List<MapData> MapDataList { get; private set; }
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
			CDB = DatabaseRead(Database.DatabaseCategory.Changeable);
			UDB = DatabaseRead(Database.DatabaseCategory.User);
			SDB = DatabaseRead(Database.DatabaseCategory.System);
			MapDataList = MapDataListRead();
			MapTree = MapTreeRead();
			TileMgr = TileSetRead();
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

			// 読込エラー処理
			if (commonEventManager == null)
			{
				AppMesOpp.AddAppMessge("コモンイベントの読込に失敗しました。");
			}

			return commonEventManager;
		}

		///<summary>DBを読込</summary>
		///<param name="db">読込対象のDB</param>
		private Database DatabaseRead(Database.DatabaseCategory dbCategory)
		{
			Database database = null;
			string projectFilePath = "";
			string datFilePath = "";
			string dbName = "";
			bool isProjectFileExist = false;
			bool isDatFileExist = false;

			switch (dbCategory)
			{
				case Database.DatabaseCategory.Changeable:
					{
						projectFilePath = Config.CDBProjrctFilePath;
						datFilePath = Config.CDBDatFilePath;
						dbName = "可変データベース";
						break;
					}
				case Database.DatabaseCategory.User:
					{
						projectFilePath = Config.UDBProjrctFilePath;
						datFilePath = Config.UDBDatFilePath;
						dbName = "ユーザーデータベース";
						break;
					}
				case Database.DatabaseCategory.System:
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

			// DB読込エラー処理
			if (database == null)
			{
				AppMesOpp.AddAppMessge($"{ dbName }の読込に失敗しました。");
			}

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
		private List<MapData> MapDataListRead()
		{
			List<MapData> mapDataList = new List<MapData>();

			// 定義ファイルの存在チェック
			if (Utils.File.CheckDirectoryExist(Config.MapDataDir, "", false))
			{
				List<string> mapFileList = Utils.File.GetFilesInDirectory(Config.MapDataDir, "*.map", true);

				foreach (var mapFile in mapFileList)
				{
					var mdr = new MapDataReader(mapFile);
					MapData mapData = mdr.GetReadData();

					if (mapData == null)
					{
						AppMesOpp.AddAppMessge($"{ Config.MapDataDir }のマップデータ読込に失敗しました。");
					}
					else
					{
						mapDataList.Add(mapData);
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

			// 読込エラー処理
			if (mapTree == null)
			{
				AppMesOpp.AddAppMessge("マップツリーの読込に失敗しました。");
			}

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

			// 読込エラー処理
			if (tileSetManager == null)
			{
				AppMesOpp.AddAppMessge("タイルセットの読込に失敗しました。");
			}

			return tileSetManager;
		}
	}
}