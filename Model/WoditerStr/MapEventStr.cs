using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WodiKs.Map.Ev;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class MapEventStr
	{
		public MapDataStr Parent { get; private set; }
		public OutputStructSentence EventID { get; private set; }
		public OutputStructSentence EventName { get; private set; }
		public OutputStructTable Position { get; private set; }
		public List<MapEventPageStr> MapEventPageList { get; private set; }

		public MapEventStr(MapEvent mapEvent, MapDataStr mapDataStr)
		{
			Parent = mapDataStr;
			EventID = new OutputStructSentence("イベントID", mapEvent.EventID.ToString());
			EventName = new OutputStructSentence("イベント名", Utils.String.Trim(mapEvent.EventName));
			Position = new OutputStructTable("座標", SetPositionHeader(), SetPositionData(mapEvent));
			MapEventPageList = SetMapEventPageList(mapEvent.EventPages, this);
		}

		private List<string> SetPositionHeader()
		{
			return new List<string>() { "X座標", "Y座標"};
		}

		private List<List<string>> SetPositionData(MapEvent mapEvent)
		{
			var positionData = new List<List<string>>();

			var record = new List<string>(new List<string>() { mapEvent.PositionX.ToString(), mapEvent.PositionY.ToString() });

			positionData.Add(record);
			return positionData;
		}

		private List<MapEventPageStr> SetMapEventPageList(MapEventPage[] mapEventPages, MapEventStr mapEventStr)
		{
			var mapEventPageList = new List<MapEventPageStr>();
			for (int pageIDNo = 0; pageIDNo < mapEventPages.Count(); pageIDNo++)
			{
				mapEventPageList.Add(new MapEventPageStr(mapEventPages[pageIDNo], pageIDNo, mapEventStr));
			}

			return mapEventPageList;
		}
	}
}
