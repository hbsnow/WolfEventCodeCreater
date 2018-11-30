using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WodiKs.DB;
using WodiKs.Ev.Common;
using WodiKs.Map;
using WodiKs.Map.Tile;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class WoditerInfoStr
	{
		public WoditerInfo Source { get; private set;}
		private Config config;
		
		///<summary>文字列化したCommonEvent情報</summary>
		public List<CommonEventStr> CEvStrs{ get; private set; }

		///<summary>文字列化したCDB情報</summary>
		public List<DatabaseTypeStr> CDBStrs { get; private set; }

		///<summary>文字列化したUDB情報</summary>
		public List<DatabaseTypeStr> UDBStrs { get; private set; }
		
		///<summary>文字列化したSDB情報</summary>
		public List<DatabaseTypeStr> SDBStrs { get; private set; }

		///<summary>文字列化したマップデータ情報</summary>
		public List<MapDataStr> MapDataStrs { get; private set; }

		///<summary>文字列化したマップツリー情報</summary>
		public MapTreeStr MapTreeStr { get; private set; }

		///<summary>文字列化したタイルセット情報</summary>
		public List<TileSetStr> TileMgrStr { get; private set; }

		public WoditerInfoStr(WoditerInfo woditerInfo, Config config)
		{
			if (woditerInfo == null)
				return;

			Source = woditerInfo;
			this.config = config;
			CEvStrs = null;
			CDBStrs = null;
			UDBStrs = null;
			SDBStrs = null;
			MapDataStrs = null;
			MapTreeStr = null;
			TileMgrStr = null;

			if (woditerInfo.CEvMgr != null)
			{
				CEvStrs = SetCEventStrs(woditerInfo.CEvMgr);
			}

			if (woditerInfo.CDB != null)
			{
				CDBStrs = SetDBTypeStrs(woditerInfo.CDB, Database.DatabaseCategory.Changeable);
			}

			if (woditerInfo.UDB != null)
			{
				UDBStrs = SetDBTypeStrs(woditerInfo.UDB, Database.DatabaseCategory.User);
			}

			if (woditerInfo.SDB != null)
			{
				SDBStrs = SetDBTypeStrs(woditerInfo.SDB, Database.DatabaseCategory.System);
			}

			if(woditerInfo.MapDataList.Count != 0)
			{
				MapDataStrs = SetMapDataStrs(woditerInfo.MapDataList);
			}

			if(woditerInfo.MapTree != null)
			{
				MapTreeStr = SeMapTreeStrs(woditerInfo.MapTree);
			}

			if(woditerInfo.TileMgr != null)
			{
				TileMgrStr = SetTileMgrStrs(woditerInfo.TileMgr);
			}
		}

		private List<CommonEventStr> SetCEventStrs(CommonEventManager commonEventManager)
		{
			List<CommonEventStr> cEventStrs = new List<CommonEventStr>();
			
			for(int cEvID = 0; cEvID < commonEventManager.NumCommonEvent; cEvID++)
			{
				CommonEvent commonEvent = commonEventManager.CommonEvents[cEvID];

				// コマンド数2未満、あるいはコモン名の入力がないもの、コメントアウトのものは除外
				string commonName = Utils.String.Trim(commonEvent.CommonEventName);
				if (commonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf(config.CommentOut) == 0)
				{
					continue;
				}

				cEventStrs.Add(new CommonEventStr(commonEvent, cEvID, Source));
			}

			return cEventStrs;
		}

		private List<DatabaseTypeStr> SetDBTypeStrs(Database db, Database.DatabaseCategory databaseCategory)
		{
			List<DatabaseTypeStr> dBStrs = new List<DatabaseTypeStr>();

			for(int typeIDNo = 0; typeIDNo < db.NumType; typeIDNo++)
			{
				// データ数0、各項目設定データが0、タイプ名の入力がないもの、コメントアウトのものは除外
				string typeName = Utils.String.Trim(db.TypesData[typeIDNo].TypeName);
				if (db.TypesData[typeIDNo].NumData == 0 || typeName == "" || typeName.IndexOf(config.CommentOut) == 0)
				{
					continue;
				}

				dBStrs.Add(new DatabaseTypeStr(db.TypesData[typeIDNo] , typeIDNo, databaseCategory, Source));
			}
			return dBStrs;
		}

		private List<MapDataStr> SetMapDataStrs(Dictionary<string, MapData> mapDataList)
		{
			List <MapDataStr> mapDataStrs = new List<MapDataStr>();

			foreach (var mapData in mapDataList)
			{
				mapDataStrs.Add(new MapDataStr(mapData.Value, mapData.Key, Source, this));
			}
			return mapDataStrs;
		}

		private MapTreeStr SeMapTreeStrs(MapTree mapTree)
		{
			/*foreach (var node in mapTree.Nodes)
			{
				System.Diagnostics.Debug.WriteLine(node.MapID, "node.MapID");
				if(node.ParentNode != null)
					System.Diagnostics.Debug.WriteLine(node.ParentNode.MapID, "node.ParentNode.MapID");
				if(node.ChildNodes != null)
					System.Diagnostics.Debug.WriteLine(node.ChildNodes.Count.ToString(), "node.ChildNodes.Count.ToString()");
				System.Diagnostics.Debug.WriteLine("---------Debug Trap---------");
			}*/

			return new MapTreeStr(mapTree, Source, this);
		}

		private List<TileSetStr> SetTileMgrStrs(TileSetManager tileMgr)
		{
			List<TileSetStr> tileSetStrs = new List<TileSetStr>();

			for(int tileSetId = 0; tileSetId < tileMgr.NumTileSet; tileSetId++)
			{
				// タイル設定名の入力がないもの、コメントアウトのものは除外
				string tileSetName = Utils.String.Trim(tileMgr.TileSets[tileSetId].Name);
				if(tileSetName == "" || tileSetName.IndexOf(config.CommentOut) == 0)
				{
					continue;
				}
				tileSetStrs.Add(new TileSetStr(tileMgr.TileSets[tileSetId], tileSetId, Source));
			}
			return tileSetStrs;
		}
	}
}
