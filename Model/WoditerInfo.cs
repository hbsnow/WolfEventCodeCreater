using System.IO;
using WodiKs.DB;
using WodiKs.Ev.Common;
using WodiKs.IO;

namespace WolfEventCodeCreater.Model
{
	public class WoditerInfo
	{
		public CommonEventManager CEvMgr { get; private set; }

		public Database CDB { get; private set; }

		public Database UDB { get; private set; }

		public Database SDB { get; private set; }

		public Config Config { get; private set; }

		public WoditerInfo(Config config)
		{
			CDB = null;
			UDB = null;
			SDB = null;
			this.Config = config;

			CommonEventRead();
			CDB = DatabaseRead(Database.DatabaseCategory.Changeable);
			UDB = DatabaseRead(Database.DatabaseCategory.User);
			SDB = DatabaseRead(Database.DatabaseCategory.System);
		}

		//TODO:コモンイベント読み込み実装
		///<summary>コモンイベントを読込</summary>
		private void CommonEventRead()
		{

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
			isProjectFileExist = Utils.File.CheckFileExist(projectFilePath , $"{ dbName }の定義ファイル(.project)");
			isDatFileExist = Utils.File.CheckFileExist(datFilePath , $"{ dbName }の定義ファイル(.dat)");

			if (isProjectFileExist && isDatFileExist)
			{
				DatabaseFileReader dfr = new DatabaseFileReader(projectFilePath , datFilePath);
				database = dfr.GetReadData();
			}

			// DB読込エラー処理
			if (database == null)
			{
				AppMesOpp.AddAppMessge($"{ dbName }の読込に失敗しました。");
			}

			return database;
		}

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
	}
}