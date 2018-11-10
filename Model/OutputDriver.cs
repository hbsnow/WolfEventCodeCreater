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
			woditerInfoStr = new WoditerInfoStr(woditerInfo);
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
			MakeOutputDir(dbCategory);

			foreach (DatabaseTypeStr databaseTypeStr in databaseTypeStrs)
			{
				// データ数0、あるいはタイプ名の入力がないもの、コメントアウトのものは除外
				if (databaseTypeStr.TypeName.Sentence.IndexOf(config.CommentOut) == 1)
				{
					continue;
				}

				List<string> outputStrs = new List<string>() { };


				
			}
		}

		///<summary>出力先ディレクトリの存在確認（存在しない場合は新たに作成）</summary>
		private void MakeOutputDir(Database.DatabaseCategory dbCategory)
		{
			string outputDir = "";
			string appMesDirName = "";

			switch (dbCategory)
			{
				case Database.DatabaseCategory.CommonEvent:
					{
						outputDir = config.CEvDumpDirPath;
						appMesDirName = "コモンイベント出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.Changeable:
					{
						outputDir = config.CDBDumpDirPath;
						appMesDirName = "可変DB出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.User:
					{
						outputDir = config.UDBDumpDirPath;
						appMesDirName = "ユーザーDB出力フォルダ";
						break;
					}
				case Database.DatabaseCategory.System:
					{
						outputDir = config.SDBDumpDirPath;
						appMesDirName = "システムDB出力フォルダ";
						break;
					}
			}
			Utils.File.CheckDirectoryExist(outputDir , appMesDirName , true);
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
						filename = ".common" + filename;
						break;
					}
				case Database.DatabaseCategory.Changeable:
					{
						outputFilePath = config.CDBDumpDirPath;
						filename = ".cdb" + filename;
						break;
					}
				case Database.DatabaseCategory.User:
					{
						outputFilePath = config.UDBDumpDirPath;
						filename = ".udb" + filename;
						break;
					}
				case Database.DatabaseCategory.System:
					{
						outputFilePath = config.SDBDumpDirPath;
						filename = ".sdb" + filename;
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

			/*    タイプIDとタイプ名    */
			list = format.FormatHeadline(list, dts.TypeID.Sentence + ":" + dts.TypeName.Sentence , 1);

			/*    メモ    */
			list = format.FormatSimpleSentence(list , dts.Memo.Sentence);

			/*    タイプの設定    */
			list = format.FormatHeadline(list , dts.TypeConfig.EntryName , 2);
			list = format.FormatTable(list , dts.TypeConfig.TableHeader , dts.TypeConfig.TableData);

			//TODO:WodiKs ver0.40 のDB読込バグが修正されるまで一旦実装を中止する

			return list;
		}


	}
}