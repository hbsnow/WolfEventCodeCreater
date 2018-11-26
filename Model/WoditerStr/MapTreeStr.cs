using System;
using System.Collections.Generic;
using WodiKs.Map;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class MapTreeStr
	{
		public WoditerInfo Source { get; private set; }
		public WoditerInfoStr SourceStr { get; private set; }
		public OutputStructTree<MapTreeNodeStr> MapTreeStrs { get; private set; }

		public MapTreeStr(MapTree mapTree, WoditerInfo woditerInfo, WoditerInfoStr woditerInfoStr)
		{
			Source = woditerInfo;
			SourceStr = woditerInfoStr;
			MapTreeStrs = new OutputStructTree<MapTreeNodeStr>("マップツリー", SetMapTreeNodesStrs(mapTree));
		}

		private List<OutputStructTreeNode<MapTreeNodeStr>> SetMapTreeNodesStrs(MapTree mapTree)
		{
			var mapTreeNodesStrs = new List<OutputStructTreeNode<MapTreeNodeStr>>();

			foreach (var mapTreeNode in mapTree.Nodes)
			{
				MapTreeNodeStr mapTreeNodeStr = new MapTreeNodeStr(mapTreeNode, this);
				mapTreeNodesStrs.Add(new OutputStructTreeNode<MapTreeNodeStr>(mapTreeNodeStr));
			}
			mapTreeNodesStrs = SetParentAndChildrenOnMapTreeStrs(mapTreeNodesStrs);

			return mapTreeNodesStrs;
		}

		private List<OutputStructTreeNode<MapTreeNodeStr>> SetParentAndChildrenOnMapTreeStrs(List<OutputStructTreeNode<MapTreeNodeStr>> mapTreeNodesStrs)
		{
			for (int nodeIndex = 0; nodeIndex < mapTreeNodesStrs.Count; nodeIndex++)
			{
				var outputStructTreeNode = mapTreeNodesStrs[nodeIndex];
				if (outputStructTreeNode.Original.ParentNode != null)
				{
					// 親ノードのMapTreeNodeStrを探索
					string mapIDOfParentNode = outputStructTreeNode.Original.ParentNode.MapID.ToString();
					OutputStructTreeNode<MapTreeNodeStr> parentMapTreeNodeStr = 
						mapTreeNodesStrs.Find(x => x.Original.MapID.Sentence == mapIDOfParentNode);

					// 親ノードと子ノードを追加
					outputStructTreeNode.ParentNode = parentMapTreeNodeStr;
					parentMapTreeNodeStr.ChildrenNode.Add(outputStructTreeNode);
				}
				else
				{
					outputStructTreeNode.ParentNode = null;
				}
			}
			return mapTreeNodesStrs;
		}
	}

	public class MapTreeNodeStr
	{
		public MapTreeStr Tree { get; private set; }
		public OutputStructSentence MapID { get; private set; }
		public OutputStructSentence MapName { get; private set; }
		public OutputStructSentence IsExpanded { get; private set; }
		public MapTreeNode ParentNode { get; private set; }

		public MapTreeNodeStr(MapTreeNode mapTreeNode, MapTreeStr mapTreeStr)
		{
			Tree = mapTreeStr;
			MapID = new OutputStructSentence("マップID", mapTreeNode.MapID.ToString());
			MapName = new OutputStructSentence("マップ名", SetMapName(mapTreeNode.MapID));
			IsExpanded = new OutputStructSentence("子ノード群が展開されているか", mapTreeNode.IsExpanded.ToString());
			ParentNode = mapTreeNode?.ParentNode;
		}

		private string SetMapName(int mapID)
		{
			// マップIDに該当するDatabaseDataStrをシステムDBから取得（\sdb[0]に対応するDatabaseDataStr.DataIDはマップIDを示す）
			if(Tree.SourceStr.SDBStrs != null)
			{
				return Tree.SourceStr.SDBStrs[0].DataList[mapID].DataName.Sentence;
			}
			else
			{
				return "";
			}
		}
	}
}