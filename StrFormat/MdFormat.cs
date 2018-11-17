using System.Collections.Generic;
using System.Data;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// MDファイル用の文字列整形クラス
	/// </summary>
	internal class MdFormat : StrFormatBase
	{
		internal MdFormat()
		{
			columnDelimiter = "|";									// 列同士の区切り文字
			betweenHeaderAndDataDelimiter = "---";		// ヘッダ部とデータ部の区切り文字
	}


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
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <param name="tableName">テーブルの名前</param>
		/// <param name="maxRowNum">テーブルのデータのうち1列に格納する最大行数</param>
		/// <param name="isSimpleSentenceWhenOnlyOneRecord">データが一行のみのときに文章に変更するかどうか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatTable(List<string> mdList, OutputStructTable outputStructTable,
			string tableName = "", int maxRowNum = 20, bool isSimpleSentenceWhenOnlyOneRecord = true)
		{
			if (outputStructTable.Columns == null || outputStructTable.Rows == null)
			{
				System.Diagnostics.Debug.WriteLine("表のヘッダまたはデータがNull");
				//TODO:Error処理
				return mdList;
			}

			if ((0 < outputStructTable.Columns.Count) && (0 < outputStructTable.Rows.Count))
			{
				if (!(isSimpleSentenceWhenOnlyOneRecord && outputStructTable.Columns.Count == 1))
				{
					mdList = this.FormatTableHeader(mdList , outputStructTable);
					mdList = this.FormatTableData(mdList , outputStructTable, maxRowNum);
					mdList = this.FormatTableFooter(mdList , "");
				}
				// headerStrsの要素が1つのみの場合は単文の文字列に整形する
				else
				{
					return FormatSimpleSentence(mdList , (string)outputStructTable.Rows[0][0]);
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("ヘッダまたはデータの要素数が0");
			}

			return mdList;
		}

		/// <summary>
		/// テーブルのヘッダの文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableHeader(List<string> mdList , OutputStructTable outputStructTable)
		{
			List<string> headerStrs = outputStructTable.Columns;
			string columnDelimiterStr = " " + columnDelimiter + " ";
			string headerStr = " ";
			string headerAndDataDelimiterStr = "";

			for (int i = 0; i < headerStrs.Count - 1; i++)
			{
				headerStr = headerStr + headerStrs[i] + columnDelimiterStr;
				headerAndDataDelimiterStr = headerAndDataDelimiterStr + betweenHeaderAndDataDelimiter + columnDelimiterStr;
			}
			// 残った最後の要素のヘッダを追加
			headerStr = headerStr + headerStrs[headerStrs.Count - 1] + " ";
			headerAndDataDelimiterStr = headerAndDataDelimiterStr + betweenHeaderAndDataDelimiter + " ";

			mdList.Add(headerStr);
			mdList.Add(headerAndDataDelimiterStr);
			
			return mdList;
		}

		/// <summary>
		/// テーブルのデータ部の文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <param name="maxRowNum">テーブルのデータのうち1列に格納する最大行数</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableData(List<string> mdList , OutputStructTable outputStructTable, int maxRowNum)
		{
			List<List<string>> dataStrs = outputStructTable.Rows;
			string columnDelimiterStr = " " + columnDelimiter + " ";

			foreach (var record in dataStrs)
			{
				string recordStr = "";
				foreach (string field in record)
				{
					/* field == ""の場合、WolfEventCodeCreaterのVer1.0.0.0の表記に合わせようとすると
					recordStrの扱いが複雑になるため、
					WolfEventCodeCreaterのVer1.0.0.0の表記に合わせないことにした。
					ただし、MDファイルをHTML表示した時は表記の違いによる影響はない。*/
					recordStr += field + columnDelimiterStr;
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