using System.Collections.Generic;
using System.Linq;
using WodiKs.Map;
using WodiKs.Map.Ev;
using WodiKs.Map.Layer;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class MapDataStr
	{
		public WoditerInfo Source { get; private set; }
		public OutputStructSentence FilePath { get; private set; }
		public OutputStructSentence MapName { get; private set; }
		public OutputStructTable MapSize { get; private set; }
		public uint Height { get; private set; }
		public uint Width { get; private set; }
		public OutputStructSentence TileSetID { get; private set; }
		public List<MapLayerStr> MapLayerList { get; private set; }
		public List<MapEventStr> MapEventList { get; private set; }

		public MapDataStr(MapData mapData, string filePath, WoditerInfo woditerInfo)
		{
			Source = woditerInfo;
			FilePath = new OutputStructSentence("ファイルパス", filePath);
			MapName = new OutputStructSentence("マップ名", SetMapName(mapData, filePath));
			MapSize = new OutputStructTable("マップサイズ", SetMapSizeHeader(), SetMapSizeData(mapData));
			Height = mapData.Height;
			Width = mapData.Width;
			TileSetID = new OutputStructSentence("タイルセットID", mapData.TileSetID.ToString());
			MapLayerList = SetMapLayerList(mapData.Layers, this);
			MapEventList = SetMapEventList(mapData.Events, this);
		}

		private string SetMapName(MapData mapData, string filePath)
		{
			string mapDataDir = Source.Config.MapDataDir;
			string filePathRemovedProjectRoot = filePath.Substring(mapDataDir.Length + @"\".Length);
			
			if (Source.SDB != null)
			{
				// システムDBからマップ名を取得（\sdb[0:n]がマップ名を、\sdb[0:n:0]がファイルパスを示す）
				WodiKs.DB.Data[] mapDataOnSDB = Source.SDB.TypesData[0].Data;
				string replacedFilePathRemovedProjectRoot = filePathRemovedProjectRoot.Replace(@"\", @"/");
				var mapNameList = mapDataOnSDB.Where(data => Utils.String.Trim(data.ItemsData[0].StringData) == replacedFilePathRemovedProjectRoot)
					.Select(data => Utils.String.Trim(data.DataName)).ToList();

				if (mapNameList.Count == 0)
				{
					return "";
				}

				string delimiter = ", ";
				string mapName = "";
				mapNameList.ForEach(x => mapName += (x + delimiter));
				return mapName.Substring(0, mapName.Length - delimiter.Length);
			}
			else
			{
				return "";
			}
		}

		private List<string> SetMapSizeHeader()
		{
			return new List<string>() { "Height", "Width", "MIN_HEIGHT", "MIN_WIDTH" };
		}

		private List<List<string>> SetMapSizeData(MapData mapData)
		{
			var data = new List<List<string>>();

			var record =
				new List<string>() { mapData.Height.ToString(), mapData.Width.ToString(), mapData.MIN_HEIGHT.ToString(), mapData.MIN_WIDTH.ToString() };

			data.Add(record);
			return data;
		}

		private List<MapLayerStr> SetMapLayerList(MapLayer[] mapLayers, MapDataStr mapDataStr)
		{
			var mapLayerList = new List<MapLayerStr>();

			for (int mapLayerId = 0; mapLayerId < mapLayers.Count(); mapLayerId++)
			{
				mapLayerList.Add(new MapLayerStr(mapLayers[mapLayerId], mapLayerId, mapDataStr));
			}

			return mapLayerList;
		}

		private List<MapEventStr> SetMapEventList(MapEvent[] mapEvents, MapDataStr mapDataStr)
		{
			var mapEventList = new List<MapEventStr>() { };

			for (int mapEventId = 0; mapEventId < mapEvents.Count(); mapEventId++)
			{
				mapEventList.Add(new MapEventStr(mapEvents[mapEventId], mapDataStr));
			}
			return mapEventList;
		}
	}
}