using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Model.OutputStruct
{
	public class OutputStructTable : OutputStructBase
	{
		///<summary>ヘッダ</summary>
		public List<string> Columns { get; private set; }

		///<summary>データ</summary>
		public List<List<string>> Rows { get; private set; }

		///<summary>レコード</summary>
		public List<string> this[int row] { get { return Rows[row]; } }

		///<summary>値</summary>
		public string this[int row, int column] { get { return Rows[row][column]; } }

		public OutputStructTable(string entryName, List<string> tableHeader, List<List<string>> tableData, bool isRenameRepeatedStrs = true)
			:base(entryName, OutputStructType.Table)
		{
			if(tableHeader == null || tableData == null)
			{
				ReturnEmpty();
				return;
			}

			// ヘッダまたはデータが空の場合は空のインスタンスを返す
			if (tableHeader.Count == 0 || tableData.Count == 0)
			{
				ReturnEmpty();
				return;
			}

			// ヘッダとデータとの列数に差異がある場合は空のインスタンスを返す
			int dataNum = 0;
			tableData.ForEach(x => dataNum += x.Count);
			if (tableHeader.Count * tableData.Count != dataNum)
			{
				System.Diagnostics.Debug.WriteLine("ヘッダとデータとの列数に差異がある");
				System.Diagnostics.Debug.WriteLine((tableHeader.Count * tableData.Count).ToString() , "tableHeader.Count * tableData.Count");
				System.Diagnostics.Debug.WriteLine(dataNum.ToString(), "dataNum");
				tableHeader.ForEach(x => System.Diagnostics.Debug.WriteLine(x , "tableHeader"));
				foreach(var strs in tableData)
				{
					string tmpStr = "tableData:[";
					strs.ForEach(x => tmpStr += (x + ", "));
					System.Diagnostics.Debug.WriteLine(tmpStr + "]");
				}
				return;
			}

			List<string> tmpTableHeader = tableHeader;

			if (isRenameRepeatedStrs)
			{
				// 重複しているヘッダの文字列を抽出（空または空白文字のみの文字列は除外）
				List<string> repeatedStrs = tableHeader.Where(x => !string.IsNullOrWhiteSpace(x)).GroupBy(g => g).Where(g => 1 < g.Count()).Select(g => g.Key).ToList();
				// 重複している文字列がある場合、当該のヘッダ文字列をリネームする
				List<int> whiteSpaceIndices = tableHeader.Select((item , index) => new { Index = index , Value = item })
					.Where(item => string.IsNullOrWhiteSpace(item.Value)).Select(item => item.Index).ToList();

				// 重複している文字列がある場合、当該のヘッダ文字列をリネームする
				if (0 < repeatedStrs.Count || 1 < whiteSpaceIndices.Count)
				{
					// 新しくインスタンスを生成（元のヘッダを変更したくないため）
					List<string> renamedRepeatedStr = new List<string>(tableHeader);

					// 重複している文字列がある場合、当該のヘッダ文字列をリネームする
					if (0 < repeatedStrs.Count)
					{
						renamedRepeatedStr = RenameRepeatedStr(renamedRepeatedStr , repeatedStrs);
					}
					// 重複している文字列が2つ以上ある場合、当該のヘッダ文字列をリネームする
					if (1 < whiteSpaceIndices.Count)
					{
						renamedRepeatedStr = RenameWhiteSpaceStr(renamedRepeatedStr, whiteSpaceIndices);
					}

					tmpTableHeader = renamedRepeatedStr;
				}
			}
			
			Columns = tmpTableHeader;

			Rows = tableData;

		}

		///<summary>空を返す</summary>
		private void ReturnEmpty()
		{
			Columns = new List<string>();
			Rows = new List<List<string>>();
		}

		///<summary>ヘッダーの値が重複している場合、リネーム</summary>
		private List<string> RenameRepeatedStr(List<string> sourceList, List<string> repeatedStrs)
		{
			foreach (string rs in repeatedStrs)
			{
				// 重複した箇所のIndex番号を取得
				var repeatedIndices = sourceList.Select((item, index) => new { Index = index, Value = item })
					.Where(item => item.Value == rs).Select(item => item.Index).ToList();
				// 連番でリネーム
				int count = 0;
				repeatedIndices.ForEach(i => sourceList[i] += (++count).ToString());
			}
			return sourceList;
		}

		///<summary>ヘッダーの値が空白の場合、リネーム</summary>
		private List<string> RenameWhiteSpaceStr(List<string> sourceList , List<int> whiteSpaceIndices)
		{
			// 2進数表記時の桁数を算出
			int figureLength = (int)(Math.Floor(Math.Log(whiteSpaceIndices.Count , 2))) + 1;

			// 2進数方式で半角空白と全角空白の組み合わせ文字列にリネーム
			for (int count = 0; count < whiteSpaceIndices.Count; count++)
			{
				// 一旦0埋めした2進数文字列に変換
				string replacedStr = Convert.ToString(count , 2).PadLeft(figureLength , '0');
				// 半角空白または全角空白文字に置換
				replacedStr = replacedStr.Replace("0" , "  ");
				replacedStr = replacedStr.Replace("1" , "　");

				// 対象のインデックスに置換文字を代入
				sourceList[whiteSpaceIndices[count]] = replacedStr;
			}
			
			return sourceList;
		}
	}
}
