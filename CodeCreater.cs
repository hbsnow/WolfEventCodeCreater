using System;
using System.IO;
using System.Collections.Generic;
using WodiKs.Ev.Common;
using WodiKs.IO;
using WolfEventCodeCreater.StrFormat;

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
                if (CommonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf("//") == 1)
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

					List<string> headerArgs = new List<string> { "Type" , "Var" , "InitialValue" , "Name" };
					List<List<string>> dataArgs = new List<List<string>>() { };
					dataArgs = PushNumericConfig(dataArgs , CommonEvent);
					dataArgs = PushStringConfig(dataArgs , CommonEvent);
					MdList = mf.FormatTable(MdList , headerArgs , dataArgs , "Args");
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
					"Cself[80~99]" , "Name[80~99]" };
				List<List<string>> dataCSelf = new List<List<string>>() { };
				dataCSelf = PushCommonSelfNames(dataCSelf , CommonEvent);
				MdList = mf.FormatTable(MdList , headerCSelf , dataCSelf , "CSelf");

				// イベントコード
				MdList = mf.FormatHeadline(MdList , "イベントコード" , 2);
				MdList.Add("```");
                MdList = PushEventCode(MdList, CommonEvent);
                MdList.Add("```");

                File.WriteAllLines(filepath, MdList);

                count++;
            }

            return $"{ count }件のMarkdownを出力しました。";
        }



		/// <summary>
		/// 起動条件データをListに追加して戻す
		/// </summary>
		/// <param name="CommonEvent"></param>
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
		/// <param name="CommonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushNumericConfig(List<List<string>> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputNumeric; i++)
            {
                var inputNumericData = commonEventConfig.InputNumerics[i];
                var initialValue = inputNumericData.InitialValue.ToString();
                var name = Utils.String.Trim(inputNumericData.Name);

                //list.Add($"数値 | \\cself[{ i }] | { initialValue } | { name }");
				List<string> recordNumericArgs = new List<string>() { "数値", $"\\cself[{ i }]", initialValue , name };
				list.Add(recordNumericArgs);
            }

            return list;
        }



		/// <summary>
		/// 文字列の引数データをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="CommonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushStringConfig(List<List<string>> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputString; i++)
            {
                var inputStringData = commonEventConfig.InputStrings[i];
                var name = Utils.String.Trim(inputStringData.Name);

                //list.Add($"文字列 | \\cself[{ i + 5 }] | | { name }");
				List<string> recordStringArgs = new List<string>() { "文字列", $"\\cself[{ i + 5 }]", "", name };
				list.Add(recordStringArgs);
			}

            return list;
        }



		/// <summary>
		/// コモンセルフ変数名データをListに追加して戻す
		/// </summary>
		/// <param name="list"></param>
		/// <param name="CommonEvent"></param>
		/// <returns></returns>
		private List<List<string>> PushCommonSelfNames(List<List<string>> list , CommonEvent commonEvent)
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
        /// コモンイベントをListに追加して戻す
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="CommonEvent"></param>
        /// <returns></returns>
        private List<string> PushEventCode(List<string> list, CommonEvent commonEvent)
        {
            list.Add("WoditorEvCOMMAND_START");

            for (int i = 0; i < commonEvent.NumEventCommand; i++)
            {
                var eventCommand = commonEvent.EventCommandList[i];

                list.Add(eventCommand.GetEventCode());
            }

            list.Add("WoditorEvCOMMAND_END");

            return list;
        }
	}
}
