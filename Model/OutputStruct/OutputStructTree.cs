using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
	public class OutputStructTree<T> : OutputStructBase where T: class
	{
		///<summary>ツリー構造のデータ</summary>
		public List<OutputStructTreeNode<T>> Nodes { get; private set; }

		public OutputStructTree(string entryName, List<OutputStructTreeNode<T>> nodeList)
			: base(entryName, OutputStructType.Tree)
		{
			Nodes = SetTreeNode(nodeList);
			/*foreach(var node in Nodes)
			{
				if(typeof(T) == typeof(WoditerStr.MapTreeNodeStr))
				{
					WoditerStr.MapTreeNodeStr mapTreeNodeStr = node.Original as WoditerStr.MapTreeNodeStr;
					System.Diagnostics.Debug.WriteLine(mapTreeNodeStr.MapID.Sentence	, "MapID");
					System.Diagnostics.Debug.WriteLine(node.Indent.ToString(), "Indent");
					System.Diagnostics.Debug.WriteLine(node.IsLastItemInChildren.ToString(), "IsLastItemInChildren");
				}
			}*/
		}

		private List<OutputStructTreeNode<T>> SetTreeNode(List<OutputStructTreeNode<T>> nodeList)
		{
			var rootList = nodeList.Where(node => node.ParentNode == null).ToList();

			for (int rootCnt = 0;rootCnt < rootList.Count; rootCnt++)
			{
				// rootのインデントを0に設定
				int firstIndent = 0;

				SetIndentAndIsLastItemInChildren(rootList, rootCnt, firstIndent);
			}


			return nodeList;
		}

		///<summary>OutputStructTreeNode&lt;T&gt;クラスのIndentとIsLastItemInChildrenを設定</summary>
		private void SetIndentAndIsLastItemInChildren(List<OutputStructTreeNode<T>> list, int index, int nowIndent)
		{
			list[index].SetIndent(nowIndent);
			if (index < list.Count - 1)
			{
				list[index].SetIsLastItemInChildren(false);
			}
			else
			{
				list[index].SetIsLastItemInChildren(true);
			}

			// 子ノードを再帰的に検査
			RecursiveCheckChildrenNode(list[index].ChildrenNode, nowIndent);
		}

		///<summary>子ノードを再帰的に検査</summary>
		private void RecursiveCheckChildrenNode(List<OutputStructTreeNode<T>> childrenNode, int prevIndent)
		{
			if (childrenNode.Count != 0)
			{
				int nowIndent = ++prevIndent;
				for (int childrenCnt = 0; childrenCnt < childrenNode.Count; childrenCnt++)
				{
					SetIndentAndIsLastItemInChildren(childrenNode, childrenCnt, nowIndent);
				}
			}
		}

	}

	///<summary>ツリー構造のノード</summary>
	public class OutputStructTreeNode<T> where T : class
	{
		public T Original { get; private set; }
		public string Value { get; private set; }
		public bool IsExpanded { get; private set; }
		public OutputStructTreeNode<T> ParentNode { get; set; }
		public List<OutputStructTreeNode<T>> ChildrenNode { get; set; }
		public int Indent { get; private set; }
		public bool IsLastItemInChildren { get; private set; }

		public OutputStructTreeNode(T original, string itemValue, bool isExpanded)
		{
			Original = original;
			Value = itemValue;
			IsExpanded = isExpanded;
			ParentNode = null;
			ChildrenNode = new List<OutputStructTreeNode<T>>();
			Indent = 0;
			IsLastItemInChildren = false;
		}

		public void SetIndent(int indent)
		{
			Indent = indent;
		}

		public void SetIsLastItemInChildren(bool isLastItemInChildren)
		{
			IsLastItemInChildren = isLastItemInChildren;
		}
	}
}
