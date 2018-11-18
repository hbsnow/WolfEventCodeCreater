using System.Collections.Generic;
using System.Data;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// MDファイル用の文字列整形クラス
	/// </summary>
	internal class MdFormat : StrFormatBase
	{
		/// <summary>
		/// 見出しの文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="headlineLevel">見出しのレベル(既定値は2)</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2)
		{
			string prefixStr = "";

			if (headlineLevel < 1)
			{
				headlineLevel = 2;
			}

			for (int i = 0; i < headlineLevel; i++)
			{
				prefixStr = prefixStr + "#";
			}

			mdList.Add($"{ prefixStr } { inputStr }\n");
			return mdList;
		}

		/// <summary>
		/// 単文の文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatSimpleSentence(List<string> mdList , string inputStr)
		{
			mdList.Add($"{ inputStr }\n");
			return mdList;
		}

		/// <summary>
		/// テーブル構造（ヘッダ部とデータ部とフッタ部）を作成し整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="headerStrs">テーブルのヘッダに適用する文字列のリスト</param>
		/// <param name="dataStrs">テーブルのデータに適用する文字列のリスト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatTable(List<string> mdList , List<string> headerStrs , List<List<string>> dataStrs , string tableName = "")
		{
			if (headerStrs == null || dataStrs == null)
			{
				System.Diagnostics.Debug.WriteLine("headerStrsまたはdataStrsがNull");
				//TODO:Error処理
				return mdList;
			}

			if ((0 < headerStrs.Count) && (0 < dataStrs.Count) && (headerStrs.Count == dataStrs[0].Count))
			{
				if (1 < headerStrs.Count)
				{
					StrTable strTable = new StrTable(headerStrs , dataStrs , "|" , "---" , tableName);
					// System.Diagnostics.Debug.WriteLine(strTable.Columns.Count, "strTable.Columns.Count");
					// System.Diagnostics.Debug.WriteLine(strTable.Rows.Count , "strTable.Rows.Count");

					mdList = this.FormatTableHeader(mdList , strTable);
					mdList = this.FormatTableData(mdList , strTable);
					mdList = this.FormatTableFooter(mdList , "");
				}
				// headerStrsの要素が1つのみの場合は単文の文字列に整形する
				else
				{
					return FormatSimpleSentence(mdList , headerStrs[0]);
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("headerStrsまたはdataStrsの要素数が0、またはheaderStrsとdataStrsとの列数に差異がある");
				//TODO:Error処理
			}

			return mdList;
		}

		/// <summary>
		/// テーブルのヘッダの文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableHeader(List<string> mdList , StrTable strTable)
		{
			DataColumnCollection headerStrs = strTable.HeaderStrs;
			string columnDelimiter = strTable.ColumnDelimiter;
			string columnDelimiterStr = " " + columnDelimiter + " ";
			string betweenHeaderAndDataDelimiter = strTable.BetweenHeaderAndDataDelimiter;
			string headerStr = " ";
			string headerAndDataDelimiterStr = "";

			if (1 < headerStrs.Count)
			{
				for (int i = 0; i < headerStrs.Count - 1; i++)
				{
					headerStr = headerStr + headerStrs[i].ColumnName + columnDelimiterStr;
					headerAndDataDelimiterStr = headerAndDataDelimiterStr + betweenHeaderAndDataDelimiter + columnDelimiterStr;
				}
				headerStr = headerStr + headerStrs[headerStrs.Count - 1].ColumnName + " ";
				headerAndDataDelimiterStr = headerAndDataDelimiterStr + betweenHeaderAndDataDelimiter + " ";

				mdList.Add(headerStr);
				mdList.Add(headerAndDataDelimiterStr);
			}
			// inputStrsの要素が1つのみの場合は単文の文字列に整形する
			else if (1 == headerStrs.Count)
			{
				return FormatSimpleSentence(mdList , headerStrs[0].ColumnName);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("headerStrsの要素数が0");
				//TODO:Error処理
				// このメソッドはprotectedかつ呼び出し元のFormatTableメソッドにてエラー処理しているため、Error処理不要だが一応残す
			}
			return mdList;
		}

		/// <summary>
		/// テーブルのデータ部の文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="dataStrs">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableData(List<string> mdList , StrTable strTable)
		{
			DataRowCollection dataStrs = strTable.DataStrs;
			string columnDelimiter = strTable.ColumnDelimiter;
			string columnDelimiterStr = " " + columnDelimiter + " ";

			foreach (DataRow record in dataStrs)
			{
				string recordStr = "";
				foreach (var field in record.ItemArray)
				{
					/* field == ""の場合、WolfEventCodeCreaterのVer1.0.0.0の表記に合わせようとすると
					recordStrの扱いが複雑になるため、
					WolfEventCodeCreaterのVer1.0.0.0の表記に合わせないことにした。
					ただし、MDファイルをHTML表示した時は表記の違いによる影響はない。*/
					recordStr += field.ToString() + columnDelimiterStr;
				}
				recordStr = recordStr.Substring(0 , recordStr.Length - columnDelimiterStr.Length);
				mdList.Add(recordStr);
			}
			return mdList;
		}

		/// <summary>
		/// テーブルのフッタ部の文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableFooter(List<string> mdList , string inputStr)
		{
			mdList.Add(inputStr);
			return mdList;
		}
	}
}