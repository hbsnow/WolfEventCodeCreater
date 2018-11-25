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

		public OutputStructTree(string entryName, List<OutputStructTreeNode<T>> nodes)
			: base(entryName, OutputStructType.Tree)
		{
			Nodes = nodes;
		}
	}

	///<summary>ツリー構造のノード</summary>
	public class OutputStructTreeNode<T> where T : class
	{
		public T Original { get; private set; }
		public T ParentNode { get; set; }
		public List<T> ChildrenNode { get; set; }

		public OutputStructTreeNode(T original)
		{
			Original = original;
			ParentNode = null;
			ChildrenNode = new List<T>();
		}
	}
}
