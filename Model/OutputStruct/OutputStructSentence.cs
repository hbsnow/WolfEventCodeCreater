using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
	public class OutputStructSentence: OutputStructBase
	{
		///<summary>文章のデータ</summary>
		public List<string> Sentences { get; private set; }

		public OutputStructSentence(string entryName, List<string> sentences) 
			:base(entryName, OutputStructType.Sentences)
		{
			Sentences = sentences;
		}
	}
}
