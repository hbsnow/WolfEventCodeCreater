using System.Collections.Generic;

namespace WolfEventCodeCreater.Model.OutputStruct
{
    public class OutputStructTables : OutputStructBase
    {
        ///<summary>OutputStructTableのList</summary>
        public List<OutputStructTable> TableList { get; private set; }

        ///<summary>OutputStructTableの個数</summary>
        public int Count { get; private set; }

        public OutputStructTables(string entryName , List<OutputStructTable> tableList)
            : base(entryName , OutputStructType.Tables)
        {
            TableList = tableList;
            Count = tableList != null ? tableList.Count : 0;
        }
    }
}