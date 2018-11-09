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
		public OutputStructSentence TypeID { get; private set; }
		public OutputStructSentence TypeName { get; private set; }
		public OutputStructSentence Memo { get; private set; }
		public OutputStructTable TypeConfig { get; private set; }
		public List<DatabaseItemConfigStr> ItemConfigList { get; private set; }
		public List<DatabaseItemStr> DataList { get; private set; }

		public DatabaseTypeStr (WodiKs.DB.Type dbType , int typeID)
		{
			TypeID = new OutputStructSentence("タイプID" , new List<string>() { typeID.ToString() });
			TypeName = new OutputStructSentence("タイプ名" , new List<string>() { Utils.String.Trim(dbType.TypeName) });
			Memo = new OutputStructSentence("メモ" , new List<string>() { Utils.String.Trim(dbType.Memo) });
			TypeConfig = new OutputStructTable("タイプ設定" , 
				new List<string>() {"データIDの設定方法" , "指定DB" , "指定タイプID" } ,SetTypeConfigData(dbType));
			SetTypeIDStr(dbType);
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

		private List<DatabaseDataIDStr> SetTypeIDStr(WodiKs.DB.Type dbType)
		{
			List<DatabaseDataIDStr> typeIDStrList = new List<DatabaseDataIDStr>();

			for (int dataIdNo = 0; dataIdNo < dbType.NumData; dataIdNo++)
			{
				typeIDStrList.Add(
					new DatabaseDataIDStr(dbType.Data[dataIdNo], dbType.NumItems, dataIdNo, dbType.ItemsConfig[dataIdNo]));
			}
			return typeIDStrList;
		}
	}
}
