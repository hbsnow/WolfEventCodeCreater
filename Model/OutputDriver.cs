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

		public  OutputDriver(Config config)
		{
			this.config = config;
			woditerInfo = new WoditerInfo(config);
			woditerInfoStr = new WoditerInfoStr(woditerInfo, config);
		}

		public void Output()
		{
			//if(CEvStrs != null)
			CreateOutputStrsCEv();
			if(woditerInfoStr.CDBStrs != null)
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

		//TODO 実装
		private void CreateOutputStrsCEv()
		{
		}

		private void CreateOutputStrsDB(List<DatabaseTypeStr> databaseTypeStrs , Database.DatabaseCategory dbCategory)
		{
			int count = 0;
			
			// 出力先ディレクトリ確認と作成
			 MakeOutputDir(dbCategory);

			foreach (DatabaseTypeStr databaseTypeStr in databaseTypeStrs)
			{
				List<string> outputStrs = new List<string>() { };

				// 各内容をList&lt;string&gt;に整形して書き出し
				outputStrs = FormatDBContents(outputStrs, databaseTypeStr);

				// 出力先ファイルパスの設定
				string outputFileName = databaseTypeStr.TypeName.Sentence;
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ databaseTypeStr.TypeID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(dbCategory, outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath , outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件の{ Utils.WodiKs.ConvertDatabaseCategoryToName(dbCategory) }のMarkdownを出力しました。");
		}

		///<summary>出力先ディレクトリの存在確認（存在しない場合は新たに作成）</summary>
		private void MakeOutputDir(Database.DatabaseCategory dbCategory)
		{
			string outputDir = "";
			//string appMesDirName = "";

			switch (dbCategory)
			{
				/*case Database.DatabaseCategory.CommonEvent:
					{
						outputDir = config.CEvDumpDirPath;
						//appMesDirName = "コモンイベント出力フォルダ";
						break;
					}*/
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
		private string ForamtToOutputFilePath(Database.DatabaseCategory dbCategory, string filenamePrefix)
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



		///<summary>DBの各内容をList&lt;string&gt;に整形して書き出し</summary>
		private List<string> FormatDBContents(List<string> list, DatabaseTypeStr dts)
		{
			MdFormat format = new MdFormat();

			/*    タイプ名    */
			list = format.FormatHeadline(list, dts.TypeName.Sentence , 1);

			/*    メモ    */
			list = format.FormatHeadline(list , dts.Memo.EntryName , 2);
			list = format.FormatSimpleSentence(list , dts.Memo.Sentence);

			/*    タイプID    */
			list = format.FormatHeadline(list , dts.TypeID.EntryName, 2);
			list = format.FormatSimpleSentence(list , dts.TypeID.Sentence);

			/*    タイプの設定    */
			list = format.FormatHeadline(list , dts.TypeConfig.EntryName , 2);
			list = format.FormatTable(list , dts.TypeConfig.TableHeader , dts.TypeConfig.TableData);

			/*    項目の設定    */
			list = format.FormatHeadline(list , "項目の設定" , 2);
			foreach(var itemConfigStr in dts.ItemConfigList)
			{
				list = format.FormatSimpleSentence(list , itemConfigStr.ItemConfigTable.EntryName);
				list = format.FormatTable(list , itemConfigStr.ItemConfigTable.TableHeader , itemConfigStr.ItemConfigTable.TableData ,
						itemConfigStr.ItemConfigTable.EntryName);

				if (itemConfigStr.ItemConfigSubTable.TableHeader.Count != 0)
				{
					list = format.FormatTable(list , itemConfigStr.ItemConfigSubTable.TableHeader , itemConfigStr.ItemConfigSubTable.TableData,
						itemConfigStr.ItemConfigSubTable.EntryName);
				}
			}

			/*    データと各項目の値    */
			list = format.FormatHeadline(list , "データと各項目の値" , 2);
			list = format.FormatHeadline(list , dts.DataTable.EntryName , 3);
			//TODO:FormatTableの行数折返し
			list = format.FormatTable(list , dts.DataTable.TableHeader , dts.DataTable.TableData);

			foreach (var data in dts.DataList)
			{
				list = format.FormatHeadline(list , $"{data.DataID.Sentence}:{data.DataName.Sentence}", 3);
				foreach(var item in data.ItemStrList)
				{
					list = format.FormatSimpleSentence(list, item.ItemTable.EntryName);
					list = format.FormatTable(list , item.ItemTable.TableHeader , item.ItemTable.TableData , item.ItemTable.EntryName);
				}
			}

			return list;
		}
	}
}