using System;
using System.Collections.Generic;
using System.Data;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// Txtファイル用の文字列整形クラス(現在未使用)
	/// </summary>
	internal class TxtFormat : StrFormatBase
	{
		internal TxtFormat()
		{
			columnDelimiter = "|";                                  // 列同士の区切り文字
			betweenHeaderAndDataDelimiter = "---";      // ヘッダ部とデータ部の区切り文字
		}

		/// <summary>
		/// 見出しの文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="headlineLevel">見出しのレベル(既定値は2)</param>
		/// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2 , bool isAddLFCodeInLastStr = true)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 単文の文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatSimpleSentence(List<string> mdList , string inputStr, bool isAddLFCodeInLastStr = true)
		{
			throw new NotImplementedException();
			/* 現在は未使用
			/// <summary>
			/// テキスト出力用の文字列に整形する
			/// </summary>
			/// <param name="entryName"></param>
			/// <param name="data"></param>
			/// <returns></returns>
			private string FormatOutputTxtStr(string entryName , string data)
			{
				return $"## { entryName } :{ data }";
			}
			*/
		}

		/// <summary>
		/// コード文の文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStrs">整形対象の文字列リスト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatCode(List<string> mdList, List<string> inputStrs)
		{
			mdList.Add("```");
			inputStrs.ForEach(str => FormatSimpleSentence(mdList, str, false));
			mdList.Add("```");
			return mdList;
		}

		/// <summary>
		/// テーブル構造（ヘッダ部とデータ部とフッタ部）を作成し整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <param name="maxRowNum">テーブルのデータのうち1列に格納する最大行数</param>
		/// <param name="isSimpleSentenceWhenOnlyOneColumn">データが一列のみのときに文章に変更するかどうか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatTable(List<string> mdList , OutputStructTable outputStructTable,
			int maxRowNum = 20, bool isSimpleSentenceWhenOnlyOneColumn = true)
		{
			if (outputStructTable.Columns == null || outputStructTable.Rows == null)
			{
				System.Diagnostics.Debug.WriteLine("表のヘッダまたはデータがNull");
				return mdList;
			}

			if ((0 < outputStructTable.Columns.Count) && (0 < outputStructTable.Rows.Count))
			{
				if (!(isSimpleSentenceWhenOnlyOneColumn && outputStructTable.Columns.Count== 1))
				{
					mdList = this.FormatTableHeader(mdList , outputStructTable);
					mdList = this.FormatTableData(mdList , outputStructTable);
					mdList = this.FormatTableFooter(mdList , "");
				}
				// データが1列のみの場合は単文の文字列に整形する
				else
				{
					foreach (var record in outputStructTable.Rows)
					{
						FormatSimpleSentence(mdList, record[0]);
					}
				}
			}
			else
			{
				// System.Diagnostics.Debug.WriteLine("ヘッダまたはデータの要素数が0");
			}

			return mdList;
		}

		/// <summary>
		/// テーブルのヘッダの文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableHeader(List<string> mdList , OutputStructTable outputStructTable)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// テーブルのデータ部の文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableData(List<string> mdList , OutputStructTable outputStructTable)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// テーブルのフッタ部の文字列に整形する【Txtファイル】
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