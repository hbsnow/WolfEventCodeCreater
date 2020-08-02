using System.Collections.Generic;

namespace WolfEventCodeCreater.Model.OutputStruct
{
    public class OutputStructSentences : OutputStructBase
    {
        ///<summary>文章のデータ</summary>
        public List<string> Sentences { get; private set; }

        ///<summary>OutputStructSentenceの個数</summary>
        public int Count { get; private set; }

        public OutputStructSentences(string entryName, List<string> sentences)
            : base(entryName, OutputStructType.Sentences)
        {
            Sentences = sentences;
            Count = sentences != null ? sentences.Count : 0;
        }
    }
}