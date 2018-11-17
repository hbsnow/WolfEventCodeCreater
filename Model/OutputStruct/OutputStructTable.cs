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

		public OutputStructTable(string entryName, List<string> tableHeader, List<List<string>> tableData):base(entryName, OutputStructType.Table)
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
				return;
			}

			List<string> tmpTableHeader = tableHeader;

			// 重複しているヘッダの文字列を抽出（空または空白文字のみの文字列は除外）
			List<string> repeatedStrs = tableHeader.Where(x => !string.IsNullOrWhiteSpace(x)).GroupBy(g => g).Where(g => 1 < g.Count()).Select(g => g.Key).ToList();

			// 重複している文字列がある場合、当該のヘッダ文字列をリネームする
			if (0 < repeatedStrs.Count)
			{
				tmpTableHeader = RenameRepeatedStr(tableHeader, repeatedStrs);
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
			// 新しくインスタンスを生成（元のヘッダを変更したくないため）
			List<string> renameRepeatedStr = new List<string>(sourceList);

			foreach (string rs in repeatedStrs)
			{
				// 重複した箇所のIndex番号を取得
				var repeatedIndexs = renameRepeatedStr.Select((item, index) => new { Index = index, Value = item })
					.Where(item => item.Value == rs).Select(item => item.Index).ToList();
				// 連番でリネーム
				int count = 0;
				repeatedIndexs.ForEach(i => renameRepeatedStr[i] += (++count).ToString());
			}
			return renameRepeatedStr;
		}
	}
}
