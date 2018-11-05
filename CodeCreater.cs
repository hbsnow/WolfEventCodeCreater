using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WodiKs.Ev.Common;
using WodiKs.IO;
using WolfEventCodeCreater.StrFormat;
using WodiKs.Ev;

namespace WolfEventCodeCreater
{
    /// <summary>
    /// Markdown生成
    /// </summary>
    public class CodeCreater
    {
        private Model.Config Config;
        private CommonEventManager CommonEventManager;
        private List<string> MdList;



        public CodeCreater(Model.Config config, CommonEventDatReader commonEventDatReader)
        {
            Config = config;
            CommonEventManager = commonEventDatReader.GetReadData();
        }



        /// <summary>
        /// Markdownファイルの出力
        /// </summary>
        /// <returns></returns>
        public string Write()
        {
            if (CommonEventManager == null)
            {
                return "ファイルがみつからない、あるいは使用中のため出力に失敗しました。";
            }

            // ファイル出力するディレクトリの作成
            if (!Directory.Exists(Config.DumpDir))
            {
                Directory.CreateDirectory(Config.DumpDir);
            }

            Console.WriteLine(Config.DumpDir);

            int count = 0;
            for (int i = 0; i < CommonEventManager.NumCommonEvent; i++)
            {
                var CommonEvent = CommonEventManager.CommonEvents[i];

                // コモン名のトリミング
                var commonName = Utils.String.Trim(CommonEvent.CommonEventName);

				// コマンド数2未満、あるいはコモン名の入力がないもの、コメントアウトのものは除外
                if (CommonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf(Config.CommentOut_Common) == 1)
                {
                    continue;
                }

                var filepath = Path.Combine(Config.DumpDir, $"{ Utils.String.FormatFilename(commonName) }.common.md");

                MdList = new List<string>();
				MdFormat mf = new MdFormat();

				MdList = mf.FormatHeadline(MdList , commonName , 1);
				MdList = mf.FormatSimpleSentence(MdList , Utils.String.Trim(CommonEvent.Memo));

				// コモンイベント色
				MdList = mf.FormatHeadline(MdList , "コモンイベント色" , 2);
				MdList = mf.FormatSimpleSentence(MdList , Utils.WodiKs.ConvertCommonEventColorToName(CommonEvent.Color));

				// 起動条件
				MdList = mf.FormatHeadline(MdList , "起動条件" , 2);
				List<string> headerTriggerConditions = new List<string> { "Type", "Var", "ComparisonValue", "ComparisonMethod"};
				List<List<string>> dataTriggerConditions = new List<List<string>>() {PushTriggerConditions(CommonEvent) };
				MdList = mf.FormatTable(MdList , headerTriggerConditions , dataTriggerConditions , "TriggerConditions");

				// 引数
				if (CommonEvent.NumInputNumeric + CommonEvent.NumInputString > 0)
                {
					MdList = mf.FormatHeadline(MdList , "引数" , 2);

					List<string> headerArgs = new List<string> { "Type" , "Var" , "InitialValue" , "Name", "SpecialSettingType" };
					List<List<string>> dataArgs = new List<List<string>>() { };
					dataArgs = PushNumericConfig(dataArgs , CommonEvent);
					dataArgs = PushStringConfig(dataArgs , CommonEvent);
					MdList = mf.FormatTable(MdList , headerArgs , dataArgs , "Args");

					// 数値入力の特殊設定
					bool isNumericSpecialSettingTypeHeadline = false;
					for (int x = 0; x < CommonEvent.NumInputNumeric; x++)
					{
						InputNumericData inputNumericData = CommonEvent.Config.InputNumerics[x];
						InputNumericData.SpecialSettingType specialSettingType = inputNumericData.SettingType;
						if(specialSettingType == InputNumericData.SpecialSettingType.ReferenceDatabase ||
							specialSettingType == InputNumericData.SpecialSettingType.ManuallyGenerateBranch)
						{
							if (!isNumericSpecialSettingTypeHeadline)
							{
								MdList = mf.FormatHeadline(MdList , "数値入力の特殊設定" , 4);
								isNumericSpecialSettingTypeHeadline = true;
							}

							if (specialSettingType == InputNumericData.SpecialSettingType.ReferenceDatabase)
							{
								MdList = mf.FormatSimpleSentence(MdList , 
									$"cself[{ x }] - {Utils.WodiKs.ConvertNumericSpecialSettingTypeToName(InputNumericData.SpecialSettingType.ReferenceDatabase)}");
								List<string> headerReferenceDatabase = new List<string> {
									"DatabaseType" , "TypeID" , "AppendItemEnable" , "-1" , "-2", "-3"};
								List<List<string>> dataReferenceDatabase = new List<List<string>>() {
									new List<string>(){
										Utils.WodiKs.ConvertDatabaseCategoryToName(inputNumericData.DatabaseType),
										inputNumericData.TypeID.ToString(), inputNumericData.AppendItemEnable.ToString(),
										Utils.String.Trim(inputNumericData.AppendItemNames[0]),
										Utils.String.Trim(inputNumericData.AppendItemNames[1]),
										Utils.String.Trim(inputNumericData.AppendItemNames[2]) } };
								MdList = mf.FormatTable(MdList , headerReferenceDatabase , dataReferenceDatabase ,
									$"\\cself[{ x.ToString() }]WithReferenceDatabase");
							}
							else
							{
								//!? InternalValueを取得できない（すべて0）。WodiKsライブラリのバグ？
								MdList = mf.FormatSimpleSentence(MdList ,
									$"cself[{ x }] - {Utils.WodiKs.ConvertNumericSpecialSettingTypeToName(InputNumericData.SpecialSettingType.ManuallyGenerateBranch)}");
								List<string> headerManuallyGenerateBranch = new List<string> {"InternalValue" , "Name"};
								List<List<string>> dataManuallyGenerateBranch = new List<List<string>>() { };
								foreach (ConfigBranch cb in inputNumericData.BranchData)
								{
									//System.Diagnostics.Debug.WriteLine(cb.InternalValue , "cb.InternalValue");
									List<string> recordManuallyGenerateBranch = new List<string>(){
										cb.InternalValue.ToString(), Utils.String.Trim(cb.DisplayString)};
									dataManuallyGenerateBranch.Add(recordManuallyGenerateBranch);
								}
								MdList = mf.FormatTable(MdList , headerManuallyGenerateBranch , dataManuallyGenerateBranch ,
									$"\\cself[{ x.ToString() }]WithManuallyGenerateBranch");
							}
						}
					}
				}

				// 返り値
				MdList = mf.FormatHeadline(MdList , "返り値" , 2);
				int returnVar = CommonEvent.Config.ReturnVariable;
				if (returnVar == -1)
				{
					MdList = mf.FormatSimpleSentence(MdList , "結果を返さない");
				}
				else
				{
					var returnValueName = Utils.String.Trim(CommonEvent.Config.ReturnValueName);
					var cselfVal = returnVar % 100;

					List<string> headerReturn = new List<string>() {"Name","Var"};
					List<string> recordReturn = new List<string>() { returnValueName , $"\\cself[{ cselfVal.ToString() }]" };
					List<List<string>> dataReturn = new List<List<string>>() { recordReturn };
					MdList = mf.FormatTable(MdList , headerReturn , dataReturn , "Return");
				}

				// コモンセルフ変数
				MdList = mf.FormatHeadline(MdList , "コモンセルフ変数" , 2);
				List<string> headerCSelf = new List<string>() {
					"Cself[0~19]" , "Name[0~19]" , "    ",
					"Cself[20~39]" , "Name[20~39]" , "  　" ,
					"Cself[40~59]" , "Name[40~59]" , "　  " ,
					"Cself[60~79]" , "Name[60~79]" , "　　" ,
					"Cself[80~99]" , "Name[80~99]" };			// DataTable型のColumsの名前は重複不可
				List<List<string>> dataCSelf = new List<List<string>>() { };
				dataCSelf = PushCommonSelfNames(dataCSelf , CommonEvent);
				MdList = mf.FormatTable(MdList , headerCSelf , dataCSelf , "CSelf");

				// イベントコード & 動作指定コマンドコード
				List<int> moveEventCommandLines = new List<int>() { };
				MdList = mf.FormatHeadline(MdList , "イベントコード" , 2);
				MdList.Add("```");
                MdList = PushEventCode(MdList, moveEventCommandLines, CommonEvent);
                MdList.Add("```");

				// 動作指定コマンドコードを追加
				if(0 < moveEventCommandLines.Count)
				{
					MdList = mf.FormatHeadline(MdList , "動作指定コマンドコード" , 2);
					foreach (int line in moveEventCommandLines)
					{
						MdList = mf.FormatSimpleSentence(MdList ,$"{ line.ToString() }行目(イベントコード)");
						// 動作指定機能フラグを取得しテーブル構造に整形
						EventCommand eventCommand = CommonEvent.EventCommandList[line - 1];
						byte moveEventFlag = eventCommand.MoveEventFlag;
						List<string> headerMoveEventFlag = new List<string>() { "動作完了までウェイト" , "動作を繰り返す" , "移動できない場合は飛ばす" };
						List<List<string>> dataMoveEventFlag = new List<List<string>>() {new List<string>(){
							Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.WaitForFinish)) ? true : false),
							Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.RepeatMovement)) ? true : false),
							Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.SkipImpossibleMoves)) ? true : false)} };
						MdList = mf.FormatTable(MdList , headerMoveEventFlag , dataMoveEventFlag);
						// あるイベントコードにおける全ての動作指定コマンドコードの最大数値データ数の取得
						int maxNumNumericData = 0;			// あるイベントコードにおける全ての動作指定コマンドコードの最大数値データ数
						maxNumNumericData = GetMaxNumNumericDataOfMoveEventCommandCode(eventCommand);
						// 動作指定コマンドコードのヘッダ部の作成
						List<string> headerMoveEventCommands = new List<string>() {"動作ID"};
						if(0 < maxNumNumericData)
						{
							for (int n = 1; n <= maxNumNumericData; n++)
							{
								headerMoveEventCommands.Add($"数値{ n }");
							}
						}
						else
						{
							headerMoveEventCommands.Add("数値なし");
							maxNumNumericData = 1;
						}
						// 動作指定コマンドコードのデータ部の作成
						List<List<string>> dataMoveEventCommands = new List<List<string>>() { };
						dataMoveEventCommands = PushMoveEventCommandCode(
							dataMoveEventCommands, CommonEvent, line, maxNumNumericData);
						// 動作指定コマンドコードをテーブル構造に整形
						MdList = mf.FormatTable(MdList, headerMoveEventCommands, dataMoveEventCommands);
					}
				}

				// 全てを出力
				File.WriteAllLines(filepath, MdList);

                count++;
            }

            return $"{ count }件のMarkdownを出力しました。";
        }



		/// <summary>
		/// 起動条件データをListに追加して戻す
		/// </summary>
		/// <param name="commonEvent"></param>
		/// <returns></returns>
		private List<string> PushTriggerConditions(CommonEvent commonEvent) {
				
			var triggerConditionsType = Utils.WodiKs.ConvertTriggerConditionsToName(commonEvent.TriggerConditionsType);
			var triggerVariable = commonEvent.TriggerVariable.ToString();
			var comparisonValue = commonEvent.ComparisonValue.ToString();
			var comparisonMethodType = Utils.WodiKs.ConvertComparisonMethodToName(commonEvent.ComparisonMethodType);

			List<string> recordTriggerConditions =
				new List<string>() { triggerConditionsType, triggerVariable, comparisonValue, comparisonMethodType };

			return recordTriggerConditions;
		}



		/// <summary>
		/// 数値の引数データをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="commonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushNumericConfig(List<List<string>> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputNumeric; i++)
            {
                var inputNumericData = commonEventConfig.InputNumerics[i];
                var initialValue = inputNumericData.InitialValue.ToString();
                var name = Utils.String.Trim(inputNumericData.Name);
				var numericSpecialSettingType = Utils.WodiKs.ConvertNumericSpecialSettingTypeToName(inputNumericData.SettingType);

				//list.Add($"数値 | \\cself[{ i }] | { initialValue } | { name }");
				List<string> recordNumericArgs = new List<string>() { "数値", $"\\cself[{ i }]", initialValue , name, numericSpecialSettingType };
				list.Add(recordNumericArgs);
            }

            return list;
        }



		/// <summary>
		/// 文字列の引数データをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="commonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushStringConfig(List<List<string>> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputString; i++)
            {
                var inputStringData = commonEventConfig.InputStrings[i];
                var name = Utils.String.Trim(inputStringData.Name);
				var stringSpecialSettingType = Utils.WodiKs.ConvertStringSpecialSettingTypeToName(inputStringData.SettingType);

				//list.Add($"文字列 | \\cself[{ i + 5 }] | | { name }");
				List<string> recordStringArgs = new List<string>() { "文字列", $"\\cself[{ i + 5 }]", "", name, stringSpecialSettingType };
				list.Add(recordStringArgs);
			}

            return list;
        }



		/// <summary>
		/// コモンセルフ変数名データをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="commonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushCommonSelfNames(List<List<string>> list, CommonEvent commonEvent)
		{
			string separator = "";
			int divideNum = commonEvent.CommonSelfNames.Length / 5;

			for (int i = 0; i < divideNum; i++)
			{
				List<string> recordCommonSelfNames = new List<string>() {
					$"cself[{(divideNum * 0 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 0 + i]), separator ,
					$"cself[{(divideNum * 1 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 1 + i]), separator ,
					$"cself[{(divideNum * 2 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 2 + i]), separator ,
					$"cself[{(divideNum * 3 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 3 + i]), separator ,
					$"cself[{(divideNum * 4 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 4 + i])};

				list.Add(recordCommonSelfNames);
			}

			return list;
		}



		/// <summary>
		/// コモンイベントコードをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="moveEventCommandLineList"></param>
		/// <param name="commonEvent"></param>
		/// <returns></returns>
		private List<string> PushEventCode(List<string> list, List<int> moveEventCommandLineList, CommonEvent commonEvent)
        {
            list.Add("WoditorEvCOMMAND_START");

            for (int i = 0; i < commonEvent.NumEventCommand; i++)
            {
                var eventCommand = commonEvent.EventCommandList[i];

				string eventCode = eventCommand.GetEventCode();

				// コモンイベントコードを出力用に最適化
				eventCode = Utils.String.RemoveDoubleCRCode(eventCode);
				eventCode = Utils.String.EncloseCRLFCodeOrSimpleLFCodeInLtAndGt(eventCode);

				list.Add(eventCode);

				// 動作指定コマンドである行を取得
				if (eventCommand.IsMoveEvent)
				{
					moveEventCommandLineList.Add(i + 1);
				}
			}

            list.Add("WoditorEvCOMMAND_END");

            return list;
        }



		/// <summary>
		/// 動作指定コマンドコードをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="commonEvent"></param>
		/// <param name="eventCodeLine"></param>
		/// <param name="maxNumNumericData"></param>
		/// <returns></returns>
		private List<List<string>> PushMoveEventCommandCode(
			List<List<string>> list , CommonEvent commonEvent , int eventCodeLine, int maxNumNumericData)
		{
			EventCommand eventCommand = commonEvent.EventCommandList[eventCodeLine - 1];
			MoveEventCommand[] moveEventCommands = eventCommand.MoveEventCommandList;

			foreach(MoveEventCommand moveEventCommand in moveEventCommands)
			{
				List<string> recordMoveEventCommandCode = new List<string>() { };

				recordMoveEventCommandCode.Add(moveEventCommand.MoveCommandID.ToString());
				
				foreach (int numData in moveEventCommand.NumericList)
				{
					recordMoveEventCommandCode.Add(numData.ToString());
				}

				// 最大数値データ数より足りない分を""で埋める
				int lackNumNumericData = maxNumNumericData - moveEventCommand.NumNumericData;
				for(int i = 0; i < lackNumNumericData; i++)
				{
					recordMoveEventCommandCode.Add("");
				}

				list.Add(recordMoveEventCommandCode);
			}

			return list;
		}



		/// <summary>
		/// 動作指定コマンドコードの数値データの最大データ数を取得する
		/// </summary>
		/// <param name="eventCommand"></param>
		/// <returns></returns>
		private int GetMaxNumNumericDataOfMoveEventCommandCode(EventCommand eventCommand)
		{
			int maxNumNumericData = 0;
			MoveEventCommand[] moveEventCommands = eventCommand.MoveEventCommandList;

			foreach (MoveEventCommand moveEventCommand in moveEventCommands)
			{
				if (maxNumNumericData < moveEventCommand.NumNumericData)
				{
					maxNumNumericData = moveEventCommand.NumNumericData;
				}
			}
			return maxNumNumericData;
		}
	}
}
