using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfEventCodeCreater.Utils;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class DatabaseTypeStr
	{
		public WoditerInfo Source { get; private set; }
		public WodiKs.DB.Database.DatabaseCategory DatabaseCategory { get; private set; }
		public OutputStructSentence TypeID { get; private set; }
		public OutputStructSentence TypeName { get; private set; }
		public OutputStructSentence Memo { get; private set; }
		public OutputStructTable TypeConfig { get; private set; }
		public List<DatabaseItemConfigStr> ItemConfigList { get; private set; }
		public OutputStructTable DataTable { get; private set; }
		public List<DatabaseDataStr> DataList { get; private set; }

		public DatabaseTypeStr (WodiKs.DB.Type dbType , int typeID, WodiKs.DB.Database.DatabaseCategory databaseCategory, WoditerInfo woditerInfo)
		{
			Source = woditerInfo;
			DatabaseCategory = databaseCategory;
			TypeID = new OutputStructSentence("タイプID" , typeID.ToString());
			TypeName = new OutputStructSentence("タイプ名" , Utils.String.Trim(dbType.TypeName));
			Memo = new OutputStructSentence("メモ" , Utils.String.Trim(dbType.Memo));
			TypeConfig = new OutputStructTable("タイプ設定" , SetTypeConfigHeader() , SetTypeConfigData(dbType));
			ItemConfigList = SetItemConfigStrList(dbType);
			DataTable = new OutputStructTable("データ", SetDataTableHeader(), SetDataTableData(dbType));
			DataList = SetDataStrList(dbType, this);
		}

		private List<string> SetTypeConfigHeader()
		{
			return new List<string>() { "データIDの設定方法" , "指定DB" , "指定タイプID" };
		}

		private List<List<string>> SetTypeConfigData(WodiKs.DB.Type dbType)
		{
			List<List<string>> data = new List<List<string>>() { };

			List<string> record = new List<string>() { };

			WodiKs.DB.TypeConfig.SettingType settingType = dbType.TypeConfig.DataIDSetting;

			record.Add(Utils.WodiKs.ConvertSettingTypeToName(settingType));

			if(settingType == WodiKs.DB.TypeConfig.SettingType.DesiredDBType)
			{
				record.Add(Utils.WodiKs.ConvertDatabaseCategoryToName(dbType.TypeConfig.DesiredDBCategory));
				record.Add(dbType.TypeConfig.DesiredTypeID.ToString());
			}
			else
			{
				record.Add("");
				record.Add("");
			}
			
			data.Add(record);
			return data;
		}

		private List<DatabaseItemConfigStr> SetItemConfigStrList(WodiKs.DB.Type dbType)
		{
			List<DatabaseItemConfigStr> itemConfigStrList = new List<DatabaseItemConfigStr>();

			for (int itemIDNo = 0; itemIDNo < dbType.NumItems; itemIDNo++)
			{
				itemConfigStrList.Add(new DatabaseItemConfigStr(dbType.ItemsConfig[itemIDNo] , itemIDNo));
			}

			return itemConfigStrList;
		}

		private List<string> SetDataTableHeader()
		{
			return new List<string>() {"DataID" , "DataName" };
		}

		private List<List<string>> SetDataTableData(WodiKs.DB.Type dbType)
		{
			List<List<string>> data = new List<List<string>>() { };

			for (int dataIdNo = 0; dataIdNo < dbType.NumData; dataIdNo++)
			{
				// タイプ設定におけるデータIDの設定方法によって、生データからデータ名を取得する
				// データIDの設定方法が手動で設定以外の場合、DataNameが""となるため
				string dataName = "";
				switch (dbType.TypeConfig.DataIDSetting)
				{
					case WodiKs.DB.TypeConfig.SettingType.Manuall:
						{
							dataName = Utils.String.Trim(dbType.Data[dataIdNo].DataName);
							break;
						}
					case WodiKs.DB.TypeConfig.SettingType.FirstStringData:
						{
							dataName = GetDataNameWithFirstStringData(dbType, dataIdNo);
							break;
						}
					case WodiKs.DB.TypeConfig.SettingType.PreviousTypeData:
						{
							dataName = GetDataNameWithPreviousTypeData(dataIdNo);
							break;
						}
					case WodiKs.DB.TypeConfig.SettingType.DesiredDBType:
						{
							dataName = GetDataNameWithDesiredDBType(dbType , dataIdNo);
							break;
						}
				}

				List<string> record = new List<string>() { dataIdNo.ToString() , dataName};
				data.Add(record);
			}
			return data;
		}

		private string GetDataNameWithFirstStringData(WodiKs.DB.Type dbType, int dataIdNo)
		{
			WodiKs.DB.ItemConfig.ItemType itemTypeOfThisItemNo0 = dbType.ItemsConfig[0].ItemDataType;

			// 項目0が文字列入力の場合
			if (itemTypeOfThisItemNo0 == WodiKs.DB.ItemConfig.ItemType.String)
			{
				string item0 = Utils.String.Trim(dbType.Data[dataIdNo].ItemsData[0].StringData);
				return item0 ?? Utils.WodiKs.NO_DATA;
			}
			else
			{
				return Utils.WodiKs.NO_DATA;
			}
		}

		private string GetDataNameWithPreviousTypeData(int dataIdNo)
		{
			if (TypeID.Sentence != "0")
			{
				WodiKs.DB.Type previousDBType = Source.GetDatabaseSource(DatabaseCategory).TypesData[int.Parse(TypeID.Sentence)];
				if (dataIdNo < previousDBType.NumData)
				{
					return Utils.String.Trim(previousDBType.Data[dataIdNo].DataName);
				}
				else
				{
					return Utils.WodiKs.NO_DATA;
				}
			}
			else
			{
				return Utils.WodiKs.NO_DATA;
			}
		}

		private string GetDataNameWithDesiredDBType(WodiKs.DB.Type dbType, int dataIdNo)
		{
			WodiKs.DB.Database.DatabaseCategory desiredDBCategory = dbType.TypeConfig.DesiredDBCategory;
			uint desiredTypeID = dbType.TypeConfig.DesiredTypeID;

			WodiKs.DB.Database desiredDB = Source.GetDatabaseSource(desiredDBCategory);

			if (desiredDB.NumType < desiredTypeID)
			{
				return Utils.WodiKs.NO_DATA;
			}

			WodiKs.DB.Type desiredType = desiredDB.TypesData[desiredTypeID];
			if (dataIdNo < desiredType.NumData)
			{
				return Utils.String.Trim(desiredType.Data[dataIdNo].DataName);
			}
			else
			{
				return Utils.WodiKs.NO_DATA;
			}
		}

		private List<DatabaseDataStr> SetDataStrList(WodiKs.DB.Type dbType, DatabaseTypeStr databaseTypeStr)
		{
			List<DatabaseDataStr> typeIDStrList = new List<DatabaseDataStr>() { };

			for (int dataIdNo = 0; dataIdNo < dbType.NumData; dataIdNo++)
			{
				typeIDStrList.Add(
					new DatabaseDataStr(dbType.Data[dataIdNo], dataIdNo , dbType.ItemsConfig, dbType.NumItems, databaseTypeStr));
			}
			return typeIDStrList;
		}
	}
}
