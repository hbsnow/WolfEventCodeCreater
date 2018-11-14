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
		public OutputStructSentence TypeID { get; private set; }
		public OutputStructSentence DataID { get; private set; }
		public OutputStructSentence DataName { get; private set; }
		public List<DatabaseItemStr> ItemStrList { get; private set; }

		public DatabaseDataStr(Data data, int dataIdNo, ItemConfig[] itemConfigs, uint itemNum, OutputStructSentence typeId)
		{
			TypeID = typeId;
			DataID = new OutputStructSentence("データID" , dataIdNo.ToString());
			DataName = new OutputStructSentence("データ名" , Utils.String.Trim(data.DataName));
			ItemStrList = SetItemStrList(data, itemConfigs, itemNum, dataIdNo);
		}

		private List<DatabaseItemStr> SetItemStrList(Data data, ItemConfig[] itemConfigs, uint itemNum, int dataIdNo)
		{
			List<DatabaseItemStr> dataList = new List<DatabaseItemStr>() { };

			for (int itemIDNo = 0; itemIDNo < itemNum; itemIDNo++)
			{
				dataList.Add(new DatabaseItemStr(data.ItemsData[itemIDNo] , itemIDNo , itemConfigs[itemIDNo] , TypeID, DataID));
			}
			return dataList;
		}
	}
}
