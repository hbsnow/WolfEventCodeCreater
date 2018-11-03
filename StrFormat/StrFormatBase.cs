using System.Collections.Generic;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// 出力ファイル用に文字列を整形するための抽象クラス
	/// </summary>
	public abstract class StrFormatBase
	{
		/// <summary>
		/// 見出しの文字列に整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="headlineLevel">見出しのレベル(既定値は2)</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public abstract List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2);

		/// <summary>
		/// 単文の文字列に整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public abstract List<string> FormatSimpleSentence(List<string> mdList , string inputStr);

		/// <summary>
		/// テーブル構造（ヘッダ部とデータ部とフッタ部）を作成し整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="headerStrs">テーブルのヘッダに適用する文字列のリスト</param>
		/// <param name="dataStrs">テーブルのデータに適用する文字列のリスト</param>
		/// <param name="tableName">テーブルの名前(既定値は"")</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public abstract List<string> FormatTable(List<string> mdList ,
			List<string> headerStrs , List<List<string>> dataStrs , string tableName = "");

		/// <summary>
		/// テーブルのヘッダ部の文字列に整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="strTable">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected abstract List<string> FormatTableHeader(List<string> mdList , StrTable strTable);

		/// <summary>
		/// テーブルのデータ部の文字列に整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="strTable">文字列のテーブルオブジェクト</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected abstract List<string> FormatTableData(List<string> mdList , StrTable strTable);

		/// <summary>
		/// テーブルのフッタ部の文字列に整形する
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected abstract List<string> FormatTableFooter(List<string> mdList , string inputStr);
	}
}