using System;
using System.Collections.Generic;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// Txtファイル用の文字列整形クラス(現在未使用)
	/// </summary>
	internal class TxtFormat : StrFormatBase
	{
		/// <summary>
		/// 見出しの文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="headlineLevel">見出しのレベル(既定値は2)</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 単文の文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatSimpleSentence(List<string> mdList , string inputStr)
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
		/// テーブル構造（ヘッダ部とデータ部とフッタ部）を作成し整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="headerStrs">テーブルのヘッダに適用する文字列のリスト</param>
		/// <param name="dataStrs">テーブルのデータに適用する文字列のリスト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatTable(List<string> mdList ,
			List<string> headerStrs , List<List<string>> dataStrs , string tableName = "")
		{
			if (headerStrs == null || dataStrs == null)
			{
				//TODO:Error処理
				return mdList;
			}

			if ((0 < headerStrs.Count) && (0 < dataStrs.Count) && (headerStrs.Count == dataStrs[0].Count))
			{
				if (1 < headerStrs.Count)
				{
					StrTable strTable = new StrTable(headerStrs , dataStrs , "|" , "---" , tableName);

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
				//TODO:Error処理
			}

			return mdList;
		}

		/// <summary>
		/// テーブルのヘッダの文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableHeader(List<string> mdList , StrTable strTable)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// テーブルのデータ部の文字列に整形する【Txtファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="dataStrs">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableData(List<string> mdList , StrTable strTable)
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