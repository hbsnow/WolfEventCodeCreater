using System.Collections.Generic;
using System.Linq;
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
		/// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2, bool isAddLFCodeInLastStr = true)
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
			string tmpStr = prefixStr + " " + inputStr;
			mdList.Add(isAddLFCodeInLastStr ? tmpStr + "\n" : tmpStr);
			return mdList;
		}

		/// <summary>
		/// 単文の文字列に整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="inputStr">整形対象の文字列</param>
		/// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatSimpleSentence(List<string> mdList , string inputStr, bool isAddLFCodeInLastStr = true)
		{
			mdList.Add(isAddLFCodeInLastStr ? inputStr + "\n" : inputStr);
			return mdList;
		}

		/// <summary>
		/// コード文の文字列に整形する【MDファイル】
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
		/// テーブル構造（ヘッダ部とデータ部とフッタ部）を作成し整形する【MDファイル】
		/// </summary>
		/// <param name="mdList">出力文字列が格納されたリスト</param>
		/// <param name="outputStructTable">出力元のテーブル構造</param>
		/// <param name="maxRowNum">テーブルのデータのうち1列に格納する最大行数</param>
		/// <param name="isSimpleSentenceWhenOnlyOneColumn">データが一列のみのときに文章に変更するかどうか</param>
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		public override List<string> FormatTable(List<string> mdList, OutputStructTable outputStructTable,
			int maxRowNum = 20, bool isSimpleSentenceWhenOnlyOneColumn = true)
		{
			if (outputStructTable.Columns == null || outputStructTable.Rows == null)
			{
				System.Diagnostics.Debug.WriteLine("表のヘッダまたはデータがNull");
				//TODO:Error処理
				return mdList;
			}

			if ((0 < outputStructTable.Columns.Count) && (0 < outputStructTable.Rows.Count))
			{
				if (!(isSimpleSentenceWhenOnlyOneColumn && outputStructTable.Columns.Count == 1))
				{
					// テーブルのデータが1列に格納する最大行数を超えた場合、折り返された新規作成のOutputStructTableを返す
					OutputStructTable tmpOutputStructTable =
						outputStructTable.Rows.Count <= maxRowNum ? outputStructTable : SetNewlyWrappedOutputStructTable(outputStructTable, maxRowNum);

					mdList = this.FormatTableHeader(mdList , tmpOutputStructTable);
					mdList = this.FormatTableData(mdList , tmpOutputStructTable);
					mdList = this.FormatTableFooter(mdList , "");
				}
				// データが1列のみの場合は単文の文字列に整形する
				else
				{
					foreach(var record in outputStructTable.Rows)
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
		/// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
		protected override List<string> FormatTableData(List<string> mdList , OutputStructTable outputStructTable)
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

		///<summary>折り返された新規作成のOutputStructTableを返す</summary>
		private OutputStructTable SetNewlyWrappedOutputStructTable(OutputStructTable sourceTable , int maxRowNum)
		{
			// 新規に折り返した結果の最終的な列セット数
			int wrappedColumnSetNum = sourceTable.Rows.Count / maxRowNum;
			if(sourceTable.Rows.Count % maxRowNum != 0)
			{
				wrappedColumnSetNum += 1;
			}

			// sourceTableの表内容のコピーを新しくインスタンス化（元のOutputStructTableを変更したくないため）
			List<string> copiedSourceHeader = new List<string>(sourceTable.Columns);
			List<List<string>> copiedSourceData = new List<List<string>>(sourceTable.Rows);

			// 第1列目（折り返す必要のない列）を初期値として代入
			List<string> newlyHeader = new List<string>(copiedSourceHeader);
			List<List<string>> newlyData = new List<List<string>>();
			for (int row = 0; row < maxRowNum; row++)
			{
				newlyData.Add(copiedSourceData[row]);
			}

			// 空欄用のデータを作成（折返したときに生じる可能性がある表の余白部分を埋めるためのデータ）
			List<string> emptyData = new List<string>();
			copiedSourceHeader.ForEach(_ => emptyData.Add(""));

			// 折返し列の代入
			for (int columnSet = 1; columnSet < wrappedColumnSetNum; columnSet++)
			{
				newlyHeader.Add("");            // 区切り用の列を追加
				newlyHeader.AddRange(copiedSourceHeader);

				for (int row = 0; row < maxRowNum; row++)
				{
					newlyData[row].Add("");            // 区切り用の列を追加

					int nowIndex = maxRowNum * columnSet + row;		// 現在forループ中で指しているインデックス
					if (nowIndex < copiedSourceData.Count)
					{
						newlyData[row].AddRange(copiedSourceData[nowIndex]);
					}
					else
					{
						newlyData[row].AddRange(emptyData);
					}
				}
			}

			OutputStructTable newlyWrappedOutputStructTable = new OutputStructTable(sourceTable.EntryName , newlyHeader, newlyData, false);

			return newlyWrappedOutputStructTable;
		}
	}
}