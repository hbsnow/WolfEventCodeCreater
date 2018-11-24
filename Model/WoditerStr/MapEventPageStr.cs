using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WodiKs.Ev;
using WodiKs.Map.Ev;
using WolfEventCodeCreater.Model.OutputStruct;
using static WodiKs.Map.Ev.MapEventPage;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class MapEventPageStr
	{
		public MapEventStr Parent { get; private set; }
		public OutputStructSentence PageID { get; private set; }
		public OutputStructTable GraphicType { get; private set; }
		public OutputStructSentence ShadowGraphicNo { get; private set; }
		public OutputStructSentence TriggerConditionsType { get; private set; }
		public OutputStructTables Triggers { get; private set; }
		public OutputStructTable ExpandCollisionRange { get; private set; }
		public OutputStructTables MovementData { get; private set; }
		public OutputStructSentences EventCommands { get; private set; }
		public List<OutputStructTables> MoveEventCommands { get; private set; }


		public MapEventPageStr(MapEventPage mapEventPage, int pageIDNo, MapEventStr mapEventStr)
		{
			Parent = mapEventStr;
			PageID = new OutputStructSentence("ページID", pageIDNo.ToString());
			GraphicType = new OutputStructTable("グラフィックの設定", SetGraphicTypeHeader(mapEventPage.GraphicType), SetGraphicTypeData(mapEventPage, mapEventPage.GraphicType));
			ShadowGraphicNo = new OutputStructSentence("影グラフィック番号", mapEventPage.ShadowGraphicNo.ToString());
			TriggerConditionsType = new OutputStructSentence("マップイベント起動条件", Utils.WodiKs.ConvertTriggerConditionsToName(mapEventPage.TriggerConditionsType));
			Triggers = new OutputStructTables("起動条件データ", SetTrigersTableList(mapEventPage));
			ExpandCollisionRange = new OutputStructTable("接触範囲拡張", SetExpandCollisionRangeHeader(), SetExpandCollisionRangeData(mapEventPage));
			MovementData = new OutputStructTables("移動情報データ", SetMovementDataTableList(mapEventPage.MovementData));
			SetEventCommandsAndMoveEventCommands(mapEventPage.EventCommandList);
		}

		private List<string> SetGraphicTypeHeader(MapEventPage.GraphicTypes graphicType)
		{
			var graphicTypeHeader = new List<string>() { "GraphicType" };

			switch (graphicType)
			{
				case MapEventPage.GraphicTypes.NoGraphic:
					{
						break;
					}
				case MapEventPage.GraphicTypes.CharaChip:
					{
						graphicTypeHeader.AddRange(new List<string>(){ "CharaAnimeNumber", "CharaDirection", "GraphicFilePath" });
						break;
					}
				case MapEventPage.GraphicTypes.TileSet:
					{
						graphicTypeHeader.AddRange(new List<string>() { "TileSetChipID", "GraphicFilePath" });
						break;
					}
			}
			return graphicTypeHeader;
		}

		private List<List<string>> SetGraphicTypeData(MapEventPage mapEventPage, MapEventPage.GraphicTypes graphicType)
		{
			var graphicTypeData = new List<List<string>>();
			var record = new List<string>() { Utils.WodiKs.ConvertGraphicTypesToName(graphicType) };

			switch (graphicType)
			{
				case MapEventPage.GraphicTypes.NoGraphic:
					{
						break;
					}
				case MapEventPage.GraphicTypes.CharaChip:
					{
						record.AddRange(new List<string>() {
							mapEventPage.CharaAnimeNumber.ToString(), mapEventPage.CharaDirection.ToString(), Utils.String.Trim(mapEventPage.GraphicFilePath) });
						break;
					}
				case MapEventPage.GraphicTypes.TileSet:
					{
						record.AddRange(new List<string>() { mapEventPage.TileSetChipID.ToString(), Utils.String.Trim(mapEventPage.GraphicFilePath) });
						break;
					}
			}
			graphicTypeData.Add(record);
			return graphicTypeData;
		}

		private List<OutputStructTable> SetTrigersTableList(MapEventPage mapEventPage)
		{
			var trigersTableList = new List<OutputStructTable>();
			if(mapEventPage.Triggers != null)
			{
				for(int id = 0; id < mapEventPage.Triggers.Count(); id++)
				{
					var trigger = mapEventPage.Triggers[id];
					var header = new List<string>() { "IsUsed", "Var", "ComparisonValue", "ComparisonMethod" };
					var data = new List<List<string>>() { new List<string>() {
						trigger.IsUsed.ToString(), trigger.TriggerVariable.ToString(),
						trigger.ComparisonValue.ToString(), Utils.WodiKs.ConvertComparisonMethodToName(trigger.ComparisonMethodType)} };
					trigersTableList.Add(new OutputStructTable($"条件{id}", header, data));
				}
			}
			return trigersTableList;
		}


		private List<string> SetExpandCollisionRangeHeader()
		{
			return new List<string>() { "X", "Y"};
		}

		private List<List<string>> SetExpandCollisionRangeData(MapEventPage mapEventPage)
		{
			var expandCollisionRangeData = new List<List<string>>();
			var record = new List<string>() { mapEventPage.ExpandCollisionRangeX.ToString(), mapEventPage.ExpandCollisionRangeY.ToString() };

			expandCollisionRangeData.Add(record);
			return expandCollisionRangeData;
		}

		private List<OutputStructTable> SetMovementDataTableList(Movement movement)
		{
			var movementDataTableList = new List<OutputStructTable>();

			// 移動タイプ、移動速度、移動頻度、アニメ速度
			var header1 = new List<string>() { "MoveType", "MoveSpeed", "MoveFrequency", "AnimationSpeed" };
			var data1 = new List<List<string>>() { new List<string>() {
				Utils.WodiKs.ConvertMoveTypesToName(movement.MoveType),
				Utils.WodiKs.ConvertMoveSpeedTypesToName(movement.MoveSpeedType),
				Utils.WodiKs.ConvertMoveFrequencyTypesToName(movement.MoveFrequencyType),
				Utils.WodiKs.ConvertAnimationSpeedTypesToName(movement.AnimationSpeedType)} };
			movementDataTableList.Add(new OutputStructTable("移動タイプ", header1, data1));

			// 移動オプション
			var header2 = new List<string>(){
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.StandbyAnimation),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.MovementAnimation),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.LockDirection),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.PassThrough),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.AboveThePlayer),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.CollisionDetection),
				Utils.WodiKs.ConvertMoveOptionFlagsToName(MoveOptionFlags.ArrangeHalfCellHigher)};
			var data2 = new List<List<string>>();
			var record2 = new List<string>();
			foreach(var moveOptionFlag in Enum.GetValues(typeof(MoveOptionFlags)))
			{
				record2.Add(Utils.String.ConvertFlagToString((0 < ((int)movement.OptionFlags & (int)moveOptionFlag)) ? true : false));
			}
			data2.Add(record2);
			movementDataTableList.Add(new OutputStructTable("移動オプション", header2, data2));

			EventCommandsStr eventCommandsStr = new EventCommandsStr();
			// 動作指定機能フラグ
			movementDataTableList.Add(eventCommandsStr.SetMoveEventFlagForMapEvent((byte)movement.MoveEventFlag));
			// 移動ルート
			movementDataTableList.Add(
				eventCommandsStr.SetMoveEventCommandsTableForMapEvent(movement.MoveEventCommandList, "移動ルート"));

			return movementDataTableList;
		}

		private void SetEventCommandsAndMoveEventCommands(EventCommand[] eventCommands)
		{
			EventCommandsStr eventCommandsStr = new EventCommandsStr();
			eventCommandsStr.SetEventCommandsAndMoveEventCommands(eventCommands, eventCommands.Count());
			EventCommands = eventCommandsStr.EventCommands;
			MoveEventCommands = eventCommandsStr.MoveEventCommands;
		}
	}
}
