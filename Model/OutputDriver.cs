using System.Collections.Generic;
using System.IO;
using WodiKs.DB;
using WolfEventCodeCreater.Model.WoditerStr;
using WolfEventCodeCreater.StrFormat;

namespace WolfEventCodeCreater.Model
{
	public class OutputDriver
	{
		private Config config;
		private WoditerInfo woditerInfo;
		private WoditerInfoStr woditerInfoStr;

		public OutputDriver(Config config)
		{
			this.config = config;
			woditerInfo = new WoditerInfo(config);
			woditerInfoStr = new WoditerInfoStr(woditerInfo , config);
		}

		///<summary>ウディタ情報をファイル出力</summary>
		public void Output()
		{
			if (woditerInfoStr.CEvStrs != null)
			{
				CreateOutputStrsCEv(woditerInfoStr.CEvStrs);
			}
			if (woditerInfoStr.CDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.CDBStrs , Database.DatabaseCategory.Changeable);
			}
			if (woditerInfoStr.UDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.UDBStrs , Database.DatabaseCategory.User);
			}
			if (woditerInfoStr.SDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.SDBStrs , Database.DatabaseCategory.System);
			}
		}

		///<summary>出力先ディレクトリの存在確認（存在しない場合は新たに作成）</summary>
		private void MakeOutputDir(Database.DatabaseCategory dbCategory)
		{
			string outputDir = "";
			//string appMesDirName = "";

			switch (dbCategory)
			{
				case Database.DatabaseCategory.CommonEvent:
					{
						outputDir = config.CEvDumpDirPath;
						//appMesDirName = "コモンイベント出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.Changeable:
					{
						outputDir = config.CDBDumpDirPath;
						//appMesDirName = "可変DB出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.User:
					{
						outputDir = config.UDBDumpDirPath;
						//appMesDirName = "ユーザーDB出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.System:
					{
						outputDir = config.SDBDumpDirPath;
						//appMesDirName = "システムDB出力フォルダ";
						break;
					}
			}
			Utils.File.CheckDirectoryExist(outputDir , "" , true);
		}

		///<summary>出力ファイルパスに整形</summary>
		private string ForamtToOutputFilePath(Database.DatabaseCategory dbCategory , string filenamePrefix)
		{
			string outputFilePath = "";
			string filename = Utils.String.FormatFilename(filenamePrefix);

			switch (dbCategory)
			{
				case Database.DatabaseCategory.CommonEvent:
					{
						outputFilePath = config.CEvDumpDirPath;
						filename += ".common";
						break;
					}
				case Database.DatabaseCategory.Changeable:
					{
						outputFilePath = config.CDBDumpDirPath;
						filename += ".cdb";
						break;
					}
				case Database.DatabaseCategory.User:
					{
						outputFilePath = config.UDBDumpDirPath;
						filename += ".udb";
						break;
					}
				case Database.DatabaseCategory.System:
					{
						outputFilePath = config.SDBDumpDirPath;
						filename += ".sdb";
						break;
					}
			}

			filename = Utils.String.AddExtension(filename);

			return outputFilePath = Path.Combine(outputFilePath , filename);
		}

		#region コモンイベント
		private void CreateOutputStrsCEv(List<CommonEventStr> CEvStrs)
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(Database.DatabaseCategory.CommonEvent);

			foreach (CommonEventStr cEvStr in CEvStrs)
			{
				List<string> outputStrs = new List<string>();

				// 各内容をList&lt;string&gt;に整形して書き出し
				outputStrs = FormatCEvContents(outputStrs , cEvStr);

				// 出力先ファイルパスの設定
				string outputFileName = cEvStr.CEvName.Sentence;
				// ファイル名にコモン番号を付ける設定対応
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ cEvStr.CEvID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(Database.DatabaseCategory.CommonEvent , outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath , outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件のコモンイベントのMarkdownを出力しました。");
		}

		///<summary>コモンイベントの内容を出力文字列に整形</summary>
		private List<string> FormatCEvContents(List<string> list , CommonEventStr ces)
		{
			MdFormat format = new MdFormat();

			/*    コモン名    */
			list = format.FormatHeadline(list , ces.CEvName.Sentence , 1);

			/*    メモ    */
			list = format.FormatSimpleSentence(list , ces.Memo.Sentence);

			/*    コモン番号    */
			list = format.FormatHeadline(list , ces.CEvID.EntryName , 2);
			list = format.FormatSimpleSentence(list , ces.CEvID.Sentence);

			/*    コモンイベント色    */
			list = format.FormatHeadline(list , ces.Color.EntryName , 2);
			list = format.FormatSimpleSentence(list , ces.Color.Sentence);

			/*    起動条件    */
			list = format.FormatHeadline(list , ces.TriggerConditions.EntryName , 2);
			list = format.FormatTable(list , ces.TriggerConditions);

			/*    引数    */
			if(ces.Args.Rows.Count != 0)
			{
				list = format.FormatHeadline(list , ces.Args.EntryName , 2);
				list = format.FormatTable(list , ces.Args);
			}

			/*    数値入力の特殊設定    */
			if (ces.NumericSpecialSettings.Count != 0)
			{
				list = format.FormatHeadline(list , ces.NumericSpecialSettings.EntryName , 4);
				foreach (var table in ces.NumericSpecialSettings.TableList)
				{
					list = format.FormatSimpleSentence(list , table.EntryName);
					list = format.FormatTable(list , table);
				}
			}

			/*    返り値    */
			list = format.FormatHeadline(list , ces.Return.EntryName , 2);
			list = ces.Return.Rows.Count == 0 ? format.FormatSimpleSentence(list , "結果を返さない") : format.FormatTable(list , ces.Return);

			/*    コモンセルフ変数    */
			list = format.FormatHeadline(list , ces.CSelf.EntryName , 2);
			list = format.FormatTable(list , ces.CSelf);

			/*    イベントコード    */
			list = format.FormatHeadline(list , ces.EventCommands.EntryName , 2);
			list.Add("```");
			foreach (var sentence in ces.EventCommands.Sentences)
			{
				list = format.FormatSimpleSentence(list , sentence, false);
			}
			list.Add("```");

			/*    動作指定コマンドコード    */
			if (ces.MoveEventCommands.Count != 0)
			{
				list = format.FormatHeadline(list , "動作指定コマンドコード" , 2);
				foreach (var tables in ces.MoveEventCommands)
				{
					list = format.FormatSimpleSentence(list , tables.EntryName);
					foreach (var table in tables.TableList)
					{
						list = format.FormatTable(list , table);
					}
				}
			}

			return list;
		}
		#endregion

		#region DB
		private void CreateOutputStrsDB(List<DatabaseTypeStr> databaseTypeStrs , Database.DatabaseCategory dbCategory)
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(dbCategory);

			foreach (DatabaseTypeStr databaseTypeStr in databaseTypeStrs)
			{
				List<string> outputStrs = new List<string>() { };

				// 各内容をList&lt;string&gt;に整形して書き出し
				outputStrs = FormatDBContents(outputStrs , databaseTypeStr);

				// 出力先ファイルパスの設定
				string outputFileName = databaseTypeStr.TypeName.Sentence;
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ databaseTypeStr.TypeID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(dbCategory , outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath , outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件の{ Utils.WodiKs.ConvertDatabaseCategoryToName(dbCategory) }のMarkdownを出力しました。");
		}

		///<summary>DBの各内容を出力文字列に整形</summary>
		private List<string> FormatDBContents(List<string> list , DatabaseTypeStr dts)
		{
			MdFormat format = new MdFormat();

			/*    タイプ名    */
			list = format.FormatHeadline(list , dts.TypeName.Sentence , 1);

			/*    メモ    */
			list = format.FormatHeadline(list , dts.Memo.EntryName , 2);
			list = format.FormatSimpleSentence(list , dts.Memo.Sentence);

			/*    DBタイプ    */
			list = format.FormatHeadline(list , "DBタイプ" , 2);
			list = format.FormatSimpleSentence(list , Utils.WodiKs.ConvertDatabaseCategoryToName(dts.DatabaseCategory));

			/*    タイプID    */
			list = format.FormatHeadline(list , dts.TypeID.EntryName , 2);
			list = format.FormatSimpleSentence(list , dts.TypeID.Sentence);

			/*    タイプの設定    */
			list = format.FormatHeadline(list , dts.TypeConfig.EntryName , 2);
			list = format.FormatTable(list , dts.TypeConfig);

			/*    項目の設定    */
			list = format.FormatHeadline(list , "項目の設定" , 2);
			foreach (var itemConfigStr in dts.ItemConfigList)
			{
				list = format.FormatHeadline(list , itemConfigStr.ItemConfigTable.EntryName, 4);
				list = format.FormatTable(list , itemConfigStr.ItemConfigTable);

				if (itemConfigStr.ItemConfigSubTable.Columns.Count != 0)
				{
					list = format.FormatTable(list , itemConfigStr.ItemConfigSubTable);
				}
			}

			/*    データと各項目の値    */
			list = format.FormatHeadline(list , "データと各項目の値" , 2);
			list = format.FormatHeadline(list , dts.DataTable.EntryName , 4);
			list = format.FormatTable(list , dts.DataTable, 20);

			foreach (var data in dts.DataList)
			{
				list = format.FormatHeadline(list , $"{data.DataID.Sentence}:{data.DataName.Sentence}" , 4);
				list = format.FormatTable(list , data.ItemAllTable, 20);
			}

			return list;
		}
		#endregion
	}
}