using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
    public class OutputStructTable : OutputStructBase
    {
        ///<summary>表形式のヘッダ部</summary>
        public List<string> TableHeader { get; private set; }

        ///<summary>表形式のデータ部</summary>
        public List<List<string>> TableData { get; private set; }

        public OutputStructTable(string entryName, List<string> tableHeader, 
            List<List<string>> tableData): base(entryName, OutputStructType.Table)
        {
            TableHeader = tableHeader;
            TableData = tableData;
        }
    }
}
