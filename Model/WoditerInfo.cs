using System.IO;
using WodiKs.DB;
using WodiKs.Ev.Common;
using WodiKs.IO;

namespace WolfEventCodeCreater.Model
{
	public class WoditerInfo
	{
		//private enum DBCategory { CDB, UDB, SDB }

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

		///<summary>DBを読込</summary>
		///<param name="db">読込対象のDB</param>
		private Database DatabaseRead(Database.DatabaseCategory dbCategory)
		{
			Database dbProperty = null;
			string projectFilePath = "";
			string datFilePath = "";
			string dbName = "";
			bool isProjectFileExist = false;
			bool isDatFileExist = false;

			switch (dbCategory)
			{
				case Database.DatabaseCategory.Changeable:
					projectFilePath = Config.CDBProjrctFilePath;
					datFilePath = Config.CDBDatFilePath;
					dbName = "可変データベース";
					break;
				case Database.DatabaseCategory.User:
					projectFilePath = Config.UDBProjrctFilePath;
					datFilePath = Config.UDBDatFilePath;
					dbName = "ユーザーデータベース";
					break;
				case Database.DatabaseCategory.System:
					projectFilePath = Config.SDBProjrctFilePath;
					datFilePath = Config.SDBDatFilePath;
					dbName = "システムデータベース";
					break;
			}

			// 定義ファイルの存在チェック
			isProjectFileExist = Utils.File.CheckFileExist(Config.CDBDatFilePath , $"{ dbName }の定義ファイル(.project)");
			isDatFileExist = Utils.File.CheckFileExist(Config.CDBDatFilePath , $"{ dbName }の定義ファイル(.dat)");

			if (isProjectFileExist && isDatFileExist)
			{
				DatabaseFileReader dfr = new DatabaseFileReader(Config.CDBProjrctFilePath , Config.CDBProjrctFilePath);
				dbProperty = dfr.GetReadData();
			}

			// DB読込エラー処理
			if (dbProperty == null)
			{
				AppMesOpp.SetAppMessge($"{ dbName }の読込に失敗しました。");
			}

			return dbProperty;
		}

		//TODO:コモンイベント読み込み実装
		///<summary>コモンイベントを読込</summary>
		private void CommonEventRead()
		{

		}
	}
}