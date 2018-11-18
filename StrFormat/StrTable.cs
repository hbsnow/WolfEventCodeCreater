using System.Collections.Generic;
using System.Data;

namespace WolfEventCodeCreater.StrFormat
{
	/// <summary>
	/// 文字列テーブルの情報が格納されているクラス
	/// </summary>
	public class StrTable : DataTable
	{
		private string columnDelimiter;         // 列同士の区切り文字
		private string betweenHeaderAndDataDelimiter;           // ヘッダ部とデータ部の区切り文字

		private StrTable()
		{
		}

		public StrTable(List<string> headerStrs , List<List<string>> dataStrs ,
			string columnDelimiter , string betweenHeaderAndDataDelimiter , string tableName) : this()
		{
			this.TableName = tableName;
			this.columnDelimiter = columnDelimiter;
			this.betweenHeaderAndDataDelimiter = betweenHeaderAndDataDelimiter;

			foreach (string headerStr in headerStrs)
			{
				this.Columns.Add(headerStr , typeof(string));
			}
			foreach (List<string> record in dataStrs)
			{
				this.Rows.Add(record.ToArray());
			}
		}

		/// <summary>
		/// テーブルのヘッダ部の文字列一覧
		/// </summary>
		public DataColumnCollection HeaderStrs { get => this.Columns; }

		/// <summary>
		/// テーブルのデータ部の文字列一覧
		/// </summary>
		public DataRowCollection DataStrs { get => this.Rows; }

		/// <summary>
		/// 列同士の区切り文字
		/// </summary>
		public string ColumnDelimiter { get => this.columnDelimiter; }

		/// <summary>
		/// ヘッダ部とデータ部の区切り文字
		/// </summary>
		public string BetweenHeaderAndDataDelimiter { get => this.betweenHeaderAndDataDelimiter; }
	}
}