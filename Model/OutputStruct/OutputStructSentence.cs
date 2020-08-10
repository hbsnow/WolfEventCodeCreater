using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
    public class OutputStructSentence : OutputStructBase
    {
        ///<summary>文章のデータ</summary>
        public string Sentence { get; private set; }

        public OutputStructSentence(string entryName , string sentence)
            : base(entryName , OutputStructType.Sentence)
        {
            Sentence = sentence;
        }
    }
}
