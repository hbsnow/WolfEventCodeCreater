using System.Collections.Generic;
using System.Linq;
using WodiKs.Map.Tile;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class TileSetStr
	{
		public WoditerInfo Source { get; private set; }
		public OutputStructSentence TileSetID { get; private set; }
		public OutputStructSentence TileSetName { get; private set; }
		public OutputStructSentence BaseFilePath { get; private set; }
		public OutputStructTable AutoTile { get; private set; }
		public OutputStructTable TileChips { get; private set; }
		//public List<TileChipStr> TileChips { get; private set; }

		public TileSetStr(TileSet tileSet, int tileSetID, WoditerInfo woditerInfo)
		{
			Source = woditerInfo;
			TileSetID = new OutputStructSentence("タイルセットID", tileSetID.ToString());
			TileSetName = new OutputStructSentence("タイルセット名", Utils.String.Trim(tileSet.Name));
			BaseFilePath = new OutputStructSentence("基本タイルセットファイルパス", Utils.String.Trim(tileSet.TilesetBaseFilePath));
			AutoTile = new OutputStructTable("オートタイルデータ", SetAutoTileHeader(), SetAutoTileData(tileSet.AutoTiles));
			TileChips = SetTileChip(tileSet.Chips);
			//TileChips = SetTileChip(tileSet.Chips);
		}

		private List<string> SetAutoTileHeader()
		{
			return new List<string>() { "ID", "ファイルパス" };
		}

		private List<List<string>> SetAutoTileData(AutoTile[] autoTiles)
		{
			List<List<string>> data = new List<List<string>>() { };

			for(int id = 0; id < autoTiles.Count(); id++)
			{
				// 空欄のオートパスは除外
				string autoTileFilePath = Utils.String.Trim(autoTiles[id].FilePath);
				if(autoTileFilePath != "")
				{
					data.Add(new List<string>() { (id + 1).ToString(), autoTileFilePath });
				}
			}
			return data;
		}

		private OutputStructTable SetTileChip(TileChip[] tileChips)
		{
			var tileChipTables = new List<OutputStructTable>();

			List<TileChipStr> tileChipStrList = SetTileChipStrs(tileChips);

			return 0 < tileChipStrList.Count ?
				new OutputStructTable("チップデータ", SetTileChipHeader(tileChipStrList[0]), SetTileChipData(tileChipStrList)) :
				new OutputStructTable("チップデータ", new List<string>(), new List<List<string>>());
		}

		private List<TileChipStr> SetTileChipStrs(TileChip[] tileChips)
		{
			List<TileChipStr> tileChipStrList = new List<TileChipStr>();

			for(int tileChipId = 0, Num = tileChips.Count(); tileChipId < Num; tileChipId++)
			{
				tileChipStrList.Add(new TileChipStr(tileChips[tileChipId], tileChipId, this));
			}
			return tileChipStrList;
		}

		private List<string> SetTileChipHeader(TileChipStr tileChipStr)
		{
			var tileChipHeader = new List<string>(){tileChipStr.PositionStr.EntryName, tileChipStr.TagID.EntryName, tileChipStr.PermissionFlag.EntryName,
					tileChipStr.PassableDirectionFlag.EntryName, tileChipStr.CounterFlag.EntryName };

			return tileChipHeader;
		}

		private List<List<string>> SetTileChipData(List<TileChipStr> tileChipStrList)
		{
			var tileChipData = new List<List<string>>();

			// 並び替え
			int tileChipStrListCount = tileChipStrList.Count;
			int maxColumnNum = 8;
			int maxRowNum = tileChipStrListCount / maxColumnNum;

			for(int index = 0; index < tileChipStrListCount; index++)
			{
				TileChipStr objTileChipStr = tileChipStrList[GetIndexOfXAxisPrioritySortedListFromIndexOfYAxisPrioritySortedList(index, maxRowNum, maxColumnNum)];

				var record = new List<string>() {objTileChipStr.PositionStr.Sentence, objTileChipStr.TagID.Sentence, objTileChipStr.PermissionFlag.Sentence,
					objTileChipStr.PassableDirectionFlag.Sentence, objTileChipStr.CounterFlag.Sentence };

				tileChipData.Add(record);
			}
			return tileChipData;
		}

		///<summary>Y軸優先リスト((0, 0) → (1, 0) → (2, 0) → ...)のインデックスをもとにX軸優先リスト((0, 0) → (0, 1) → (0, 2) → ...)のインデックスを取得する</summary>
		private int GetIndexOfXAxisPrioritySortedListFromIndexOfYAxisPrioritySortedList(int indexOfYAxisPrioritySortedList, int maxRowNum, int maxColumnNum)
		{
			int PositonX_IndexOfYAxisPrioritySortedList = indexOfYAxisPrioritySortedList / maxRowNum;
			int PositonY_IndexOfYAxisPrioritySortedList = indexOfYAxisPrioritySortedList % maxRowNum;

			return PositonY_IndexOfYAxisPrioritySortedList * maxColumnNum + PositonX_IndexOfYAxisPrioritySortedList;
		}
	}
}