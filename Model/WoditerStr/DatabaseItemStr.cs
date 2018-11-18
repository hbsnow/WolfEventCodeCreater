using System;
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
		public OutputStructSentence DataID { get; private set; }
		public OutputStructSentence ItemID { get; private set; }
		public OutputStructTable DataTable { get; private set; }

		public DatabaseItemStr(ItemData itemData, int itemIDNo, ItemConfig itemConfig, int dataIDNo)
		{
			DataID = new OutputStructSentence("DataID", dataIDNo.ToString());
			ItemID = new OutputStructSentence("項目ID" , itemIDNo.ToString());
			DataTable = SetDataTable(itemData, itemIDNo, itemConfig);
		}

		private OutputStructTable SetDataTable(ItemData itemData, int itemIDNo, ItemConfig itemConfig)
		{
			return new OutputStructTable($"項目{itemIDNo.ToString()}",
				SetDataTableHeader(), SetDataTableData(itemData, itemConfig));
		}


		private List<string> SetDataTableHeader()
		{
			List<string> dataTableHeader = new List<string>() {
				"ItemName","ValueType" ,"Value"};

			return dataTableHeader;
		}

		private List<List<string>> SetDataTableData(ItemData itemData, ItemConfig itemConfig)
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
