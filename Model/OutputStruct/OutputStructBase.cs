using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
	public enum OutputStructType { Sentence, Sentences, Table, Tables, Mix}

	class OutputStructBase
	{
		///<summary>項目名</summary>
		public string EntryName { get; private set; }

		public OutputStructType StructType { get; private set; }

		protected OutputStructBase(string entryName, OutputStructType outputStructTypeout)
		{
			EntryName = entryName;
			StructType = outputStructTypeout;
		}
	}
}
