using System;
using System.Collections.Generic;
using System.IO;
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
		public WoditerInfoStr SourceStr { get; private set; }
		public OutputStructSentence FilePath { get; private set; }
		public string FileName { get { return Path.GetFileName(FilePath.Sentence);} }
		public OutputStructSentence MapID { get; private set; }
		public OutputStructSentence MapName { get; private set; }
		public OutputStructTable MapSize { get; private set; }
		public uint Height { get; private set; }
		public uint Width { get; private set; }
		public OutputStructSentence TileSetID { get; private set; }
		public OutputStructTables MapLayerTables { get; private set; }
		public List<MapEventStr> MapEventList { get; private set; }

		public MapDataStr(MapData mapData, string filePath, WoditerInfo woditerInfo, WoditerInfoStr woditerInfoStr)
		{
			Source = woditerInfo;
			SourceStr = woditerInfoStr;
			FilePath = new OutputStructSentence("ファイルパス", filePath);
			MapID = new OutputStructSentence("マップID", SetMapID(mapData, filePath));
			MapName = new OutputStructSentence("マップ名", SetMapName(mapData, filePath));
			MapSize = new OutputStructTable("マップサイズ", SetMapSizeHeader(), SetMapSizeData(mapData));
			Height = mapData.Height;
			Width = mapData.Width;
			TileSetID = new OutputStructSentence("タイルセットID", mapData.TileSetID.ToString());
			MapLayerTables = new OutputStructTables("マップレイヤー", SetMapLayerTables(mapData.Layers));
			MapEventList = SetMapEventList(mapData.Events, this);
		}

		private List<DatabaseDataStr> GetSDBDataStrOfMap(MapData mapData, string filePath)
		{
			string mapDataDir = Source.Config.MapDataDir;
			string filePathRemovedProjectRoot = filePath.Substring(mapDataDir.Length + @"\".Length);

			if (SourceStr.SDBStrs != null)
			{
				if(SourceStr.SDBStrs.Count == 0)
				{
					return new List<DatabaseDataStr>();
				}
				// マップデータに該当するDatabaseDataStrをシステムDBから取得（dataStr.ItemStrList[0].ItemTable[0][3]はSDBの項目:マップファイル名の値を示す）
				List<DatabaseDataStr> mapDataListOnSDB = SourceStr.SDBStrs[0].DataList;
				string replacedFilePathRemovedProjectRoot = filePathRemovedProjectRoot.Replace(@"\", @"/");
				return mapDataListOnSDB.Where(dataStr => dataStr.ItemStrList[0].ItemTable[0][3] == replacedFilePathRemovedProjectRoot)
					.Select(dataStr => dataStr).ToList();
			}
			else
			{
				return new List<DatabaseDataStr>();
			}
		}

		private string SetMapID(MapData mapData, string filePath)
		{
			List<DatabaseDataStr> dataStrList = GetSDBDataStrOfMap(mapData, filePath);

			if(dataStrList.Count != 0)
			{
				string delimiter = ", ";
				string mapID = "";
				dataStrList.Select(dataStr => dataStr.DataID.Sentence).ToList().ForEach(id => mapID += (id + delimiter));

				return mapID.Substring(0, mapID.Length - delimiter.Length);
			}
			else
			{
				return "";
			}
		}

		private string SetMapName(MapData mapData, string filePath)
		{
			List<DatabaseDataStr> dataStrList = GetSDBDataStrOfMap(mapData, filePath);

			if (dataStrList.Count != 0)
			{
				string delimiter = ", ";
				string mapName = "";
				dataStrList.Select(dataStr => dataStr.DataName.Sentence).ToList().ForEach(name => mapName += (name + delimiter));

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

		private List<OutputStructTable> SetMapLayerTables(MapLayer[] mapLayers)
		{
			var mapLayerTableList = new List<OutputStructTable>();

			for (int mapLayerId = 0; mapLayerId < mapLayers.Count(); mapLayerId++)
			{
				mapLayerTableList.Add(
					new OutputStructTable($"レイヤー{(mapLayerId + 1).ToString()}の各チップID",
					SetMapLayerHeader(), SetMapLayerData(mapLayers[mapLayerId])));
			}

			return mapLayerTableList;
		}

		private List<string> SetMapLayerHeader()
		{
			var layerChipsHeader = new List<string>() { " x→   y↓" };		// 表左上の凡例
			for (int x = 0; x < Width; x++)
			{
				layerChipsHeader.Add(x.ToString());
			}
			return layerChipsHeader;
		}

		private List<List<string>> SetMapLayerData(MapLayer mapLayer)
		{
			var layerChipsData = new List<List<string>>();
			long rowNum = mapLayer.LayerChips.Count() / Width;
			long columnNum = mapLayer.LayerChips.Count() / Height;

			for (long y = 0; y < rowNum; y++)
			{
				var record = new List<string>() { y.ToString() };		// Y座標の番号
				for (long x = 0; x < columnNum; x++)
				{
					int layerChipsIndex = (int)(x * Height + y);        // LayerChips:左上端座標(0,0)の位置から下方向への連番チップ:(0, 0) → (0, 1) → (0, 2) → ...
					string chipIdStr = mapLayer.LayerChips[layerChipsIndex].ChipID.ToString();
					/*if(chipIdStr == "100000")
					{
						chipIdStr = "None";
					}*/
					record.Add(chipIdStr);
				}
				layerChipsData.Add(record);
			}

			return layerChipsData;
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