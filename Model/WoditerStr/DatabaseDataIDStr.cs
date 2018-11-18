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
	public class DatabaseDataIDStr
	{
		public OutputStructSentence DataID { get; private set; }
		public OutputStructSentence DataName { get; private set; }
		public List<DatabaseItemConfigStr> ItemConfigStrList { get; private set; }
		public List<DatabaseItemStr> DataStrList { get; private set; }

		public DatabaseDataIDStr(Data data, uint itemNum, int dataIdNo, ItemConfig itemConfig)
		{
			DataID = new OutputStructSentence("データID" ,  dataIdNo.ToString());
			DataName = new OutputStructSentence("データ名" , Utils.String.Trim(data.DataName));
			ItemConfigStrList = SetItemConfigStrList(itemConfig, itemNum);
			DataStrList = SetItemStrList(data, itemNum, itemConfig, dataIdNo);
		}

		private List<DatabaseItemConfigStr> SetItemConfigStrList(ItemConfig itemConfig, uint itemNum)
		{
			List<DatabaseItemConfigStr> itemConfigStrList = new List<DatabaseItemConfigStr>();

			for (int itemNo = 0; itemNo < itemNum; itemNo++)
			{
				itemConfigStrList.Add(new DatabaseItemConfigStr(itemConfig, itemNo));
			}

			return itemConfigStrList;
		}

		private List<DatabaseItemStr> SetItemStrList(Data data, uint itemNum, ItemConfig itemConfig, int dataIdNo)
		{
			List<DatabaseItemStr> dataList = new List<DatabaseItemStr>() { };

			for (int itemNo = 0; itemNo < itemNum; itemNo++)
			{
				dataList.Add(new DatabaseItemStr(data.ItemsData[itemNo], itemNo, itemConfig, dataIdNo));
			}
			return dataList;
		}
	}
}
