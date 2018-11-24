using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfEventCodeCreater.Utils;
using WolfEventCodeCreater.Model.OutputStruct;
using WodiKs.DB;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class DatabaseDataStr
	{
		///<summary>このデータを所有するタイプ</summary>
		public DatabaseTypeStr Parent { get; private set; }
		///<summary>データID</summary>
		public OutputStructSentence DataID { get; private set; }
		///<summary>データ名</summary>
		public OutputStructSentence DataName { get; private set; }
		///<summary>このデータが持つ各項目のリスト</summary>
		public List<DatabaseItemStr> ItemStrList { get; private set; }
		///<summary>ItemStrListの各項目情報をまとめたテーブル</summary>
		public OutputStructTable ItemAllTable { get; private set; }

		public DatabaseDataStr(Data data, int dataIdNo, ItemConfig[] itemConfigs, uint itemNum, DatabaseTypeStr parent)
		{
			Parent = parent;
			DataID = new OutputStructSentence("データID" , dataIdNo.ToString());
			DataName = new OutputStructSentence("データ名" , parent.DataTable[dataIdNo][1]);
			ItemStrList = SetItemStrList(data, itemConfigs, itemNum, dataIdNo);
			ItemAllTable = SetItemAllTable();
		}

		private List<DatabaseItemStr> SetItemStrList(Data data, ItemConfig[] itemConfigs, uint itemNum, int dataIdNo)
		{
			List<DatabaseItemStr> dataList = new List<DatabaseItemStr>() { };

			for (int itemIDNo = 0; itemIDNo < itemNum; itemIDNo++)
			{
				dataList.Add(new DatabaseItemStr(data.ItemsData[itemIDNo] , itemIDNo , itemConfigs[itemIDNo] , this));
			}
			return dataList;
		}

		private OutputStructTable SetItemAllTable()
		{
			List<string> itemAllTableHeader = new List<string>();
			List<List<string>> itemAllTableData = new List<List<string>>();

			if (ItemStrList.Count != 0)
			{
				itemAllTableHeader = ItemStrList[0].ItemTable.Columns;
				foreach(var itemStr in ItemStrList)
				itemAllTableData.Add(itemStr.ItemTable.Rows[0]);
			}

			OutputStructTable itemTable = new OutputStructTable("項目値", itemAllTableHeader , itemAllTableData);
			return itemTable;
		}
	}
}
