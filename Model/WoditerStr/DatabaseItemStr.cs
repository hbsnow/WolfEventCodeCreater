﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfEventCodeCreater.Model.OutputStruct;
using WodiKs.DB;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class DatabaseItemStr
	{
		public DatabaseDataStr Parent { get; private set; }
		public OutputStructSentence ItemID { get; private set; }
		public OutputStructTable ItemTable { get; private set; }

		public DatabaseItemStr(ItemData itemData, int itemIDNo, ItemConfig itemConfig, DatabaseDataStr parent)
		{
			Parent = parent;
			ItemID = new OutputStructSentence("項目ID" , itemIDNo.ToString());
			ItemTable = SetItemTable(itemData, itemIDNo, itemConfig);
		}

		private OutputStructTable SetItemTable(ItemData itemData, int itemIDNo, ItemConfig itemConfig)
		{
			return new OutputStructTable($"項目{itemIDNo.ToString()}",
				SetItemTableHeader(), SetItemTableData(itemData, itemConfig));
		}


		private List<string> SetItemTableHeader()
		{
			List<string> dataTableHeader = new List<string>() {
				"ItemName","ValueType" ,"Value"};

			return dataTableHeader;
		}

		private List<List<string>> SetItemTableData(ItemData itemData, ItemConfig itemConfig)
		{
			List<List<string>> dataTableData = new List<List<string>>() { };

			List<string> record = new List<string>() { };

			record.Add(Utils.String.Trim(itemConfig.ItemName));        // ItemName
			record.Add(Utils.WodiKs.ConvertItemTypeToName(itemConfig.ItemDataType));    // ValueType

			if (itemConfig.ItemDataType == ItemConfig.ItemType.Numeric)
			{
				record.Add(itemData.NumericData.ToString());        // Value
			}
			else
			{
				record.Add(Utils.String.Trim(itemData.StringData));        // Value
			}

			dataTableData.Add(record);

			return dataTableData;
		}
	}
}
