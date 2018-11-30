using System.Collections.Generic;
using System.IO;
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
			woditerInfoStr = new WoditerInfoStr(woditerInfo, config);
		}

		///<summary>ウディタ情報をファイル出力</summary>
		public void Output()
		{
			MdFormat format = new MdFormat();

			if (woditerInfoStr.CEvStrs != null)
			{
				CreateOutputStrsCEv(woditerInfoStr.CEvStrs, format);
			}
			if (woditerInfoStr.CDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.CDBStrs, WoditerInfo.WoditerInfoCategory.CDB, format);
			}
			if (woditerInfoStr.UDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.UDBStrs, WoditerInfo.WoditerInfoCategory.UDB, format);
			}
			if (woditerInfoStr.SDBStrs != null)
			{
				CreateOutputStrsDB(woditerInfoStr.SDBStrs, WoditerInfo.WoditerInfoCategory.SDB, format);
			}

			if(woditerInfoStr.MapDataStrs.Count != 0)
			{
				CreateOutputStrsMap(woditerInfoStr.MapDataStrs, format);
			}

			if(woditerInfoStr.MapTreeStr != null)
			{
				CreateOutputStrsMapTree(woditerInfoStr.MapTreeStr, format);
			}

			if(woditerInfoStr.TileMgrStr != null)
			{
				CreateOutputStrsTileSet(woditerInfoStr.TileMgrStr, format);
			}
		}

		///<summary>出力先ディレクトリの存在確認（存在しない場合は新たに作成）</summary>
		private void MakeOutputDir(WoditerInfo.WoditerInfoCategory woditerInfoCategory)
		{
			Utils.File.CheckDirectoryExist(config.GetOutputDir(woditerInfoCategory), "", true);
		}

		///<summary>出力ファイルパスに整形</summary>
		private string ForamtToOutputFilePath(WoditerInfo.WoditerInfoCategory woditerInfoCategory, string filenamePrefix)
		{
			string outputFilePath = config.GetOutputDir(woditerInfoCategory);
			string filename = Utils.String.FormatFilename(filenamePrefix);

			switch (woditerInfoCategory)
			{
				case WoditerInfo.WoditerInfoCategory.CEv:
					{
						filename += ".common";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.CDB:
					{
						filename += ".cdb";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.UDB:
					{
						filename += ".udb";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.SDB:
					{
						filename += ".sdb";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.Map:
					{
						filename += ".map";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.MapTree:
					{
						filename += ".maptree";
						break;
					}
				case WoditerInfo.WoditerInfoCategory.TileSet:
					{
						filename += ".tileset";
						break;
					}
			}
			filename = Utils.String.AddExtension(filename);

			return outputFilePath = Path.Combine(outputFilePath, filename);
		}

		#region コモンイベント

		private void CreateOutputStrsCEv<Format>(List<CommonEventStr> CEvStrs, Format format) where Format : StrFormatBase
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(WoditerInfo.WoditerInfoCategory.CEv);

			foreach (CommonEventStr cEvStr in CEvStrs)
			{
				List<string> outputStrs = new List<string>();

				// 各内容をList<string>に整形して書き出し
				outputStrs = FormatCEvContents(outputStrs, cEvStr, format);

				// 出力先ファイルパスの設定
				string outputFileName = cEvStr.CEvName.Sentence;
				// ファイル名にコモン番号を付ける設定対応
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ cEvStr.CEvID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(WoditerInfo.WoditerInfoCategory.CEv, outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath, outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件のコモンイベントのMarkdownを出力しました。");
		}

		///<summary>コモンイベントの内容を出力文字列に整形</summary>
		private List<string> FormatCEvContents<Format>(List<string> list, CommonEventStr ces, Format format) where Format : StrFormatBase
		{
			/*    コモン名    */
			list = format.FormatHeadline(list, ces.CEvName.Sentence, 1);

			/*    メモ    */
			list = format.FormatSimpleSentence(list, ces.Memo.Sentence);

			/*    コモン番号    */
			list = format.FormatHeadline(list, ces.CEvID.EntryName, 2);
			list = format.FormatSimpleSentence(list, ces.CEvID.Sentence);

			/*    コモンイベント色    */
			list = format.FormatHeadline(list, ces.Color.EntryName, 2);
			list = format.FormatSimpleSentence(list, ces.Color.Sentence);

			/*    起動条件    */
			list = format.FormatHeadline(list, ces.TriggerConditions.EntryName, 2);
			list = format.FormatTable(list, ces.TriggerConditions);

			/*    引数    */
			if (ces.Args.Rows.Count != 0)
			{
				list = format.FormatHeadline(list, ces.Args.EntryName, 2);
				list = format.FormatTable(list, ces.Args);
			}

			/*    数値入力の特殊設定    */
			if (ces.NumericSpecialSettings.Count != 0)
			{
				list = format.FormatHeadline(list, ces.NumericSpecialSettings.EntryName, 4);
				foreach (var table in ces.NumericSpecialSettings.TableList)
				{
					list = format.FormatSimpleSentence(list, table.EntryName);
					list = format.FormatTable(list, table);
				}
			}

			/*    返り値    */
			list = format.FormatHeadline(list, ces.Return.EntryName, 2);
			list = ces.Return.Rows.Count == 0 ? format.FormatSimpleSentence(list, "結果を返さない") : format.FormatTable(list, ces.Return);

			/*    コモンセルフ変数    */
			list = format.FormatHeadline(list, ces.CSelf.EntryName, 2);
			list = format.FormatTable(list, ces.CSelf);

			/*    イベントコード    */
			list = format.FormatHeadline(list, ces.EventCommands.EntryName, 2);
			list = format.FormatCode(list, ces.EventCommands.Sentences);

			/*    動作指定コマンドコード    */
			if (ces.MoveEventCommands.Count != 0)
			{
				list = format.FormatHeadline(list, "動作指定コマンドコード", 2);
				foreach (var tables in ces.MoveEventCommands)
				{
					list = format.FormatSimpleSentence(list, tables.EntryName);
					foreach (var table in tables.TableList)
					{
						list = format.FormatTable(list, table);
					}
				}
			}

			return list;
		}

		#endregion コモンイベント

		#region DB

		private void CreateOutputStrsDB<Format>
			(List<DatabaseTypeStr> databaseTypeStrs, WoditerInfo.WoditerInfoCategory woditerInfoCategory, Format format) where Format : StrFormatBase
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(woditerInfoCategory);

			foreach (DatabaseTypeStr databaseTypeStr in databaseTypeStrs)
			{
				List<string> outputStrs = new List<string>() { };

				// 各内容をList&lt;string&gt;に整形して書き出し
				outputStrs = FormatDBContents(outputStrs, databaseTypeStr, format);

				// 出力先ファイルパスの設定
				string outputFileName = databaseTypeStr.TypeName.Sentence;
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ databaseTypeStr.TypeID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(woditerInfoCategory, outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath, outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件の{ woditerInfoCategory.ToString() }のMarkdownを出力しました。");
		}

		///<summary>DBの各内容を出力文字列に整形</summary>
		private List<string> FormatDBContents<Format>(List<string> list, DatabaseTypeStr dts, Format format) where Format : StrFormatBase
		{
			/*    タイプ名    */
			list = format.FormatHeadline(list, dts.TypeName.Sentence, 1);

			/*    メモ    */
			list = format.FormatHeadline(list, dts.Memo.EntryName, 2);
			list = format.FormatSimpleSentence(list, dts.Memo.Sentence);

			/*    DBタイプ    */
			list = format.FormatHeadline(list, "DBタイプ", 2);
			list = format.FormatSimpleSentence(list, Utils.WodiKs.ConvertDatabaseCategoryToName(dts.DatabaseCategory));

			/*    タイプID    */
			list = format.FormatHeadline(list, dts.TypeID.EntryName, 2);
			list = format.FormatSimpleSentence(list, dts.TypeID.Sentence);

			/*    タイプの設定    */
			list = format.FormatHeadline(list, dts.TypeConfig.EntryName, 2);
			list = format.FormatTable(list, dts.TypeConfig);

			/*    項目の設定    */
			list = format.FormatHeadline(list, "項目の設定", 2);
			foreach (var itemConfigStr in dts.ItemConfigList)
			{
				list = format.FormatHeadline(list, itemConfigStr.ItemConfigTable.EntryName, 4);
				list = format.FormatTable(list, itemConfigStr.ItemConfigTable);

				if (itemConfigStr.ItemConfigSubTable.Columns.Count != 0)
				{
					list = format.FormatTable(list, itemConfigStr.ItemConfigSubTable);
				}
			}

			/*    データと各項目の値    */
			list = format.FormatHeadline(list, "データと各項目の値", 2);
			list = format.FormatHeadline(list, dts.DataTable.EntryName, 4);
			list = format.FormatTable(list, dts.DataTable, 20);

			foreach (var data in dts.DataList)
			{
				list = format.FormatHeadline(list, $"{data.DataID.Sentence}:{data.DataName.Sentence}", 4);
				list = format.FormatTable(list, data.ItemAllTable, 20);
			}

			return list;
		}

		#endregion DB

		#region マップ

		private void CreateOutputStrsMap<Format>(List<MapDataStr> mapDataList, Format format) where Format : StrFormatBase
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(WoditerInfo.WoditerInfoCategory.Map);

			foreach (var mapData in mapDataList)
			{
				List<string> outputStrs = new List<string>();

				// 各内容をList<string>に整形して書き出し
				outputStrs = FormatMapContents(outputStrs, mapData, format);

				// 出力先ファイルパスの設定
				string outputFileName = Path.GetFileNameWithoutExtension(mapData.FilePath.Sentence);
				// ファイル名にコモン番号を付ける設定対応
				if (config.IsOutputCommonNumber)
				{
					outputFileName = $"{ mapData.MapID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(WoditerInfo.WoditerInfoCategory.Map, outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath, outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件のマップのMarkdownを出力しました。");
		}

		///<summary>マップデータの内容を出力文字列に整形</summary>
		private List<string> FormatMapContents<Format>(List<string> list, MapDataStr mds, Format format) where Format : StrFormatBase
		{
			/*    マップファイル名    */
			list = format.FormatHeadline(list, mds.FileName, 1);

			/*    マップ名    */
			list = format.FormatHeadline(list, mds.MapName.EntryName, 2);
			list = format.FormatSimpleSentence(list, mds.MapName.Sentence);

			/*    マップID    */
			list = format.FormatHeadline(list, mds.MapID.EntryName, 2);
			list = format.FormatSimpleSentence(list, mds.MapID.Sentence);

			/*    マップサイズ    */
			list = format.FormatHeadline(list, mds.MapSize.EntryName, 2);
			list = format.FormatTable(list, mds.MapSize);

			/*    タイルセットID    */
			list = format.FormatHeadline(list, mds.TileSetID.EntryName, 2);
			list = format.FormatSimpleSentence(list, mds.TileSetID.Sentence);

			/*    レイヤー    */
			list = format.FormatHeadline(list, mds.MapLayerTables.EntryName, 2);
			foreach (var mapLayer in mds.MapLayerTables.TableList)
			{
				list = format.FormatHeadline(list, mapLayer.EntryName, 4);
				list = format.FormatTable(list, mapLayer, 10000);
			}

			/*    マップイベント    */
			list = format.FormatHeadline(list, "マップイベント", 1);
			foreach(var mapEvent in mds.MapEventList)
			{
				/*    マップイベント名    */
				list = format.FormatHeadline(list, mapEvent.EventName.EntryName, 2);
				list = format.FormatSimpleSentence(list, mapEvent.EventName.Sentence);

				/*    マップイベントID    */
				list = format.FormatHeadline(list, mapEvent.EventID.EntryName, 4);
				list = format.FormatSimpleSentence(list, mapEvent.EventID.Sentence);

				/*    座標    */
				list = format.FormatHeadline(list, mapEvent.Position.EntryName, 4);
				list = format.FormatTable(list, mapEvent.Position);

				/*    マップイベントページ    */
				list = format.FormatHeadline(list, "マップイベントページ", 4);
				foreach(var mapEventPage in mapEvent.MapEventPageList)
				{
					/*    マップイベントページID    */
					list = format.FormatHeadline(list, mapEventPage.PageID.EntryName, 5);
					list = format.FormatSimpleSentence(list, mapEventPage.PageID.Sentence);

					/*    グラフィックの設定    */
					list = format.FormatHeadline(list, mapEventPage.GraphicType.EntryName, 5);
					list = format.FormatTable(list, mapEventPage.GraphicType);

					/*    影グラフィック番号    */
					list = format.FormatHeadline(list, mapEventPage.ShadowGraphicNo.EntryName, 5);
					list = format.FormatSimpleSentence(list, mapEventPage.ShadowGraphicNo.Sentence);

					/*    マップイベント起動条件    */
					list = format.FormatHeadline(list, mapEventPage.TriggerConditionsType.EntryName, 5);
					list = format.FormatSimpleSentence(list, mapEventPage.TriggerConditionsType.Sentence);

					/*    起動条件データ    */
					list = format.FormatHeadline(list, mapEventPage.Triggers.EntryName, 5);
					foreach(var trigger in mapEventPage.Triggers.TableList)
					{
						list = format.FormatSimpleSentence(list, trigger.EntryName);
						list = format.FormatTable(list, trigger);
					}

					/*    接触範囲拡張    */
					list = format.FormatHeadline(list, mapEventPage.ExpandCollisionRange.EntryName, 5);
					list = format.FormatTable(list, mapEventPage.ExpandCollisionRange);

					/*    移動情報データ    */
					list = format.FormatHeadline(list, mapEventPage.MovementData.EntryName, 5);
					foreach (var movementData in mapEventPage.MovementData.TableList)
					{
						if(movementData.Rows.Count != 0)
						{
							list = format.FormatSimpleSentence(list, movementData.EntryName);
							list = format.FormatTable(list, movementData);
						}
					}

					/*    マップイベントコード    */
					if(2 < mapEventPage.EventCommands.Sentences.Count)
					{
						list = format.FormatHeadline(list, mapEventPage.EventCommands.EntryName, 5);
						list = format.FormatCode(list, mapEventPage.EventCommands.Sentences);

						/*    動作指定コマンドコード    */
						if (mapEventPage.MoveEventCommands.Count != 0)
						{
							list = format.FormatHeadline(list, "動作指定コマンドコード", 5);
							foreach (var moveEventCommandTables in mapEventPage.MoveEventCommands)
							{
								list = format.FormatSimpleSentence(list, moveEventCommandTables.EntryName);
								foreach(var moveEventCommandTable in moveEventCommandTables.TableList)
								list = format.FormatTable(list, moveEventCommandTable);
							}
						}
					}
				}
			}

			return list;
		}

		#endregion マップ

		#region マップツリー

		private void CreateOutputStrsMapTree<Format>(MapTreeStr mapTreeStr, Format format) where Format : StrFormatBase
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(WoditerInfo.WoditerInfoCategory.MapTree);

			List<string> outputStrs = new List<string>();

			/*    マップツリー    */
			outputStrs = format.FormatHeadline(outputStrs, mapTreeStr.MapTreeStrs.EntryName, 1);
			outputStrs = format.FormatTree(outputStrs, mapTreeStr.MapTreeStrs);

			// 出力先ファイルパスの設定
			string outputFileName = "MapTree";
			string outputFilePath = ForamtToOutputFilePath(WoditerInfo.WoditerInfoCategory.MapTree, outputFileName);

			// 出力
			File.WriteAllLines(outputFilePath, outputStrs);
			count++;

			AppMesOpp.AddAppMessge($"{ count }件のマップツリーのMarkdownを出力しました。");
		}

		#endregion

		#region タイルセット

		private void CreateOutputStrsTileSet<Format>(List<TileSetStr> tileMgrStr, Format format) where Format : StrFormatBase
		{
			int count = 0;

			// 出力先ディレクトリ確認と作成
			MakeOutputDir(WoditerInfo.WoditerInfoCategory.TileSet);

			foreach(TileSetStr tileSetStr in tileMgrStr)
			{
				List<string> outputStrs = new List<string>();

				// 各内容をList<string>に整形して書き出し
				outputStrs = FormatTileSetContents(outputStrs, tileSetStr, format);

				// 出力先ファイルパスの設定
				string outputFileName = tileSetStr.TileSetName.Sentence;
				// ファイル名にコモン番号を付ける設定対応
				if(config.IsOutputCommonNumber)
				{
					outputFileName = $"{ tileSetStr.TileSetID.Sentence }_{ outputFileName }";
				}
				string outputFilePath = ForamtToOutputFilePath(WoditerInfo.WoditerInfoCategory.TileSet, outputFileName);

				// 出力
				File.WriteAllLines(outputFilePath, outputStrs);
				count++;
			}

			AppMesOpp.AddAppMessge($"{ count }件のタイルセットのMarkdownを出力しました。");
		}

		///<summary>タイルセットの内容を出力文字列に整形</summary>
		private List<string> FormatTileSetContents<Format>(List<string> list, TileSetStr tileSetStr, Format format) where Format : StrFormatBase
		{
			/*    タイルセット名    */
			list = format.FormatHeadline(list, tileSetStr.TileSetName.Sentence, 1);

			/*    タイルセットID    */
			list = format.FormatHeadline(list, tileSetStr.TileSetID.EntryName, 2);
			list = format.FormatSimpleSentence(list, tileSetStr.TileSetID.Sentence);

			/*    基本タイルセットファイルパス    */
			list = format.FormatHeadline(list, tileSetStr.BaseFilePath.EntryName, 2);
			list = format.FormatSimpleSentence(list, tileSetStr.BaseFilePath.Sentence);

			/*    オートタイルデータ    */
			if(0 < tileSetStr.AutoTile.Rows.Count)
			{
				list = format.FormatHeadline(list, tileSetStr.AutoTile.EntryName, 2);
				list = format.FormatTable(list, tileSetStr.AutoTile);
			}

			/*    チップデータ(チップタグID&通行許可設定&通行方向設定&カウンター属性設定)    */
			list = format.FormatHeadline(list, tileSetStr.TileChips.EntryName, 2);
			list = format.FormatTable(list, tileSetStr.TileChips, tileSetStr.TileChips.Rows.Count / 8);

			return list;
		}

		#endregion
	}
}