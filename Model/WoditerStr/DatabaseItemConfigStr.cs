using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfEventCodeCreater.Model.OutputStruct;
using WodiKs.DB;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class DatabaseItemConfigStr
	{
		public int ItemIDNo { get; private set; }
		public OutputStructTable ItemConfigTable { get; private set; }
		public OutputStructTable ItemConfigSubTable { get; private set; }


		public DatabaseItemConfigStr(ItemConfig itemConfig , int itemIdNo)
		{
			ItemIDNo = itemIdNo;
			ItemConfigTable = SetItemConfigTable(itemConfig, itemIdNo);
			ItemConfigSubTable = SetItemConfigSubTable(itemConfig);
		}

		private OutputStructTable SetItemConfigTable(ItemConfig itemConfig, int itemIdNo)
		{
			return new OutputStructTable($"項目{itemIdNo.ToString()}の設定データ",
				SetItemConfigTableHeader(itemConfig), SetItemConfigTableData(itemConfig));
		}


		private List<string> SetItemConfigTableHeader(ItemConfig itemConfig)
		{
			List<string> itemConfigTableHeader = new List<string>() {"ItemName",
				"ItemDataType" ,"InitialValue","SpecialSettingType"};

			return itemConfigTableHeader;
		}

		private List<List<string>> SetItemConfigTableData(ItemConfig itemConfig)
		{
			List<List<string>> itemConfigTableData = new List<List<string>>() { };

			List<string> record = new List<string>() {Utils.String.Trim(itemConfig.ItemName),
				Utils.WodiKs.ConvertItemTypeToName(itemConfig.ItemDataType),
				itemConfig.InitialValue.ToString(),
				Utils.WodiKs.ConvertItemConfigSpecialSettingTypeToName(itemConfig.SettingType)};

			itemConfigTableData.Add(record);

			return itemConfigTableData;
		}

		private OutputStructTable SetItemConfigSubTable(ItemConfig itemConfig)
		{
			return new OutputStructTable(
				Utils.WodiKs.ConvertItemConfigSpecialSettingTypeToName(itemConfig.SettingType),
				SetItemConfigSubTableHeader(itemConfig), SetItemConfigSubTableData(itemConfig));
		}

		private List<string> SetItemConfigSubTableHeader(ItemConfig itemConfig)
		{
			List<string> itemConfigSubTableHeader = new List<string>() { };

			switch (itemConfig.SettingType)
			{
				case ItemConfig.SpecialSettingType.NotUse:
					// No Operation
					break;
				case ItemConfig.SpecialSettingType.ReadFile:
					itemConfigSubTableHeader.AddRange(new List<string>() { "フォルダパス", "保存時はフォルダ名を省くか" });
					break;
				case ItemConfig.SpecialSettingType.ReferenceDatabase:
					itemConfigSubTableHeader.AddRange(new List<string>() { "DatabaseType", "TypeID", "AppendItemEnable", "-1", "-2", "-3" });
					break;
				case ItemConfig.SpecialSettingType.ManuallyGenerateBranch:
					itemConfigSubTableHeader.AddRange(new List<string>() { "選択肢の内部値", "表示文字列" });
					break;
			}
			return itemConfigSubTableHeader;
		}

		private List<List<string>> SetItemConfigSubTableData(ItemConfig itemConfig)
		{
			List<List<string>> itemConfigSubTableData = new List<List<string>>() { };

			switch (itemConfig.SettingType)
			{
				case ItemConfig.SpecialSettingType.NotUse:
					itemConfigSubTableData.Add(new List<string>() { });
					break;
				case ItemConfig.SpecialSettingType.ReadFile:
					itemConfigSubTableData.Add(new List<string>() {
						Utils.String.Trim(itemConfig.FolderPath), itemConfig.OmitFolderNameEnable.ToString() });
					break;
				case ItemConfig.SpecialSettingType.ReferenceDatabase:
					itemConfigSubTableData.Add(new List<string>() {
						Utils.WodiKs.ConvertDatabaseCategoryToName(itemConfig.DatabaseType),
						itemConfig.TypeID.ToString(), itemConfig.AppendItemEnable.ToString(),
						Utils.String.Trim(itemConfig.AppendItemNames[0]),
						Utils.String.Trim(itemConfig.AppendItemNames[1]),
						Utils.String.Trim(itemConfig.AppendItemNames[2])});
					break;
				case ItemConfig.SpecialSettingType.ManuallyGenerateBranch:
					foreach(ItemConfigBranch branch in itemConfig.BranchData)
					{
						List<string> record = 
							new List<string>() { branch.InternalValue.ToString(), Utils.String.Trim(branch.DisplayString) };
						itemConfigSubTableData.Add(record);
					}
					break;
			}
			return itemConfigSubTableData;
		}
	}
}
