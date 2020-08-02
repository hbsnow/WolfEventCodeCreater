using System.Collections.Generic;
using WodiKs.Ev;
using WodiKs.Ev.Common;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
    public class CommonEventStr
    {
        ///<summary>ウディタ情報</summary>
        public WoditerInfo Source { get; private set; }

        ///<summary>コモンイベントID</summary>
        public OutputStructSentence CEvID { get; private set; }

        ///<summary>コモン名</summary>
        public OutputStructSentence CEvName { get; private set; }

        ///<summary>メモ</summary>
        public OutputStructSentence Memo { get; private set; }

        ///<summary>コモンイベント色</summary>
        public OutputStructSentence Color { get; private set; }

        ///<summary>起動条件</summary>
        public OutputStructTable TriggerConditions { get; private set; }

        ///<summary>引数</summary>
        public OutputStructTable Args { get; private set; }

        ///<summary>数値入力の特殊設定</summary>
        public OutputStructTables NumericSpecialSettings { get; private set; }

        ///<summary>返り値</summary>
        public OutputStructTable Return { get; private set; }

        ///<summary>コモンセルフ変数</summary>
        public OutputStructTable CSelf { get; private set; }

        ///<summary>イベントコード</summary>
        public OutputStructSentences EventCommands { get; private set; }

        ///<summary>動作指定コマンドコード</summary>
        public List<OutputStructTables> MoveEventCommands { get; private set; }

        public CommonEventStr(CommonEvent commonEvent , int cEvID , WoditerInfo woditerInfo)
        {
            Source = woditerInfo;
            CEvID = new OutputStructSentence("コモン番号" , System.String.Format("{0:000}" , cEvID));
            CEvName = new OutputStructSentence("コモン名" , Utils.String.Trim(commonEvent.CommonEventName));
            Memo = new OutputStructSentence("メモ" , Utils.String.Trim(commonEvent.Memo));
            Color = new OutputStructSentence("コモンイベント色" , Utils.WodiKs.ConvertCommonEventColorToName(commonEvent.Color));
            TriggerConditions = new OutputStructTable("起動条件" , SetTriggerConditionsHeader() , SetTriggerConditionsData(commonEvent));
            Args = new OutputStructTable("引数" , SetArgsHeader() , SetArgsData(commonEvent));
            NumericSpecialSettings = SetNumericSpecialSettings(commonEvent);
            Return = new OutputStructTable("返り値" , SetReturnHeader() , SetReturnData(commonEvent));
            CSelf = new OutputStructTable("コモンセルフ変数" , SetCSelfHeader() , SetCSelfData(commonEvent));
            SetEventCommandsAndMoveEventCommands(commonEvent);
        }

        #region 起動条件

        private List<string> SetTriggerConditionsHeader()
        {
            List<string> headerTriggerConditions = new List<string> { "Type" , "Var" , "ComparisonValue" , "ComparisonMethod" };
            return headerTriggerConditions;
        }

        private List<List<string>> SetTriggerConditionsData(CommonEvent commonEvent)
        {
            List<List<string>> dataTriggerConditions = new List<List<string>>();

            var triggerConditionsType = Utils.WodiKs.ConvertTriggerConditionsToName(commonEvent.TriggerConditionsType);
            var triggerVariable = commonEvent.TriggerVariable.ToString();
            var comparisonValue = commonEvent.ComparisonValue.ToString();
            var comparisonMethodType = Utils.WodiKs.ConvertComparisonMethodToName(commonEvent.ComparisonMethodType);

            List<string> recordTriggerConditions =
                new List<string>() { triggerConditionsType , triggerVariable , comparisonValue , comparisonMethodType };

            dataTriggerConditions.Add(recordTriggerConditions);
            return dataTriggerConditions;
        }

        #endregion 起動条件

        #region 引数

        private List<string> SetArgsHeader()
        {
            List<string> headerArgs = new List<string> { "Type" , "Var" , "InitialValue" , "Name" , "SpecialSettingType" };
            return headerArgs;
        }

        private List<List<string>> SetArgsData(CommonEvent commonEvent)
        {
            List<List<string>> dataArgs = new List<List<string>>();
            // 数値入力
            if (0 < commonEvent.NumInputNumeric)
            {
                dataArgs = PushNumericConfig(dataArgs , commonEvent);
            }
            // 文字列入力
            if (0 < commonEvent.NumInputString)
            {
                dataArgs = PushStringConfig(dataArgs , commonEvent);
            }

            return dataArgs;
        }

        /// <summary>
        /// 数値の引数データをListに追加して戻す
        /// </summary>
        /// <param name="list"></param>
        /// <param name="commonEvent"></param>
        /// <returns></returns>
        private List<List<string>> PushNumericConfig(List<List<string>> list , CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputNumeric; i++)
            {
                var inputNumericData = commonEventConfig.InputNumerics[i];
                var initialValue = inputNumericData.InitialValue.ToString();
                var name = Utils.String.Trim(inputNumericData.Name);
                var numericSpecialSettingType = Utils.WodiKs.ConvertNumericSpecialSettingTypeToName(inputNumericData.SettingType);

                List<string> recordNumericArgs = new List<string>() { "数値" , $"\\cself[{ i }]" , initialValue , name , numericSpecialSettingType };
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
        private List<List<string>> PushStringConfig(List<List<string>> list , CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputString; i++)
            {
                var inputStringData = commonEventConfig.InputStrings[i];
                var name = Utils.String.Trim(inputStringData.Name);
                var stringSpecialSettingType = Utils.WodiKs.ConvertStringSpecialSettingTypeToName(inputStringData.SettingType);

                //list.Add($"文字列 | \\cself[{ i + 5 }] | | { name }");
                List<string> recordStringArgs = new List<string>() { "文字列" , $"\\cself[{ i + 5 }]" , "" , name , stringSpecialSettingType };
                list.Add(recordStringArgs);
            }

            return list;
        }

        #endregion 引数

        #region 数値入力の特殊設定

        private OutputStructTables SetNumericSpecialSettings(CommonEvent commonEvent)
        {
            List<OutputStructTable> numericSpecialSettingTables = new List<OutputStructTable>();

            if (0 < commonEvent.NumInputNumeric)
            {
                for (int x = 0; x < commonEvent.NumInputNumeric; x++)
                {
                    OutputStructTable recordTable = SetNumericSpecialSettingTable(commonEvent.Config.InputNumerics[x] , x);
                    if (recordTable.Rows.Count != 0)
                    {
                        numericSpecialSettingTables.Add(recordTable);
                    }
                }
            }
            OutputStructTables numericSpecialSettings = new OutputStructTables("数値入力の特殊設定" , numericSpecialSettingTables);

            return numericSpecialSettings;
        }

        private OutputStructTable SetNumericSpecialSettingTable(InputNumericData inputNumericData , int inputNumericNo)
        {
            List<string> header = new List<string>();
            List<List<string>> data = new List<List<string>>();

            InputNumericData.SpecialSettingType specialSettingType = inputNumericData.SettingType;

            switch (specialSettingType)
            {
                case InputNumericData.SpecialSettingType.NotUse:
                    {
                        break;
                    }
                case InputNumericData.SpecialSettingType.ReferenceDatabase:
                    {
                        header.AddRange(new List<string> { "DatabaseType" , "TypeID" , "AppendItemEnable" , "-1" , "-2" , "-3" });
                        data.Add(new List<string>(){
                                        Utils.WodiKs.ConvertDatabaseCategoryToName(inputNumericData.DatabaseType),
                                        inputNumericData.TypeID.ToString(), inputNumericData.AppendItemEnable.ToString(),
                                        Utils.String.Trim(inputNumericData.AppendItemNames[0]),
                                        Utils.String.Trim(inputNumericData.AppendItemNames[1]),
                                        Utils.String.Trim(inputNumericData.AppendItemNames[2])});
                        break;
                    }
                case InputNumericData.SpecialSettingType.ManuallyGenerateBranch:
                    {
                        header.AddRange(new List<string> { "InternalValue" , "Name" });
                        foreach (ConfigBranch cb in inputNumericData.BranchData)
                        {
                            List<string> recordManuallyGenerateBranch = new List<string>(){
                                        cb.InternalValue.ToString(), Utils.String.Trim(cb.DisplayString)};
                            data.Add(recordManuallyGenerateBranch);
                        }
                        break;
                    }
                default:
                    break;
            }

            string entryName = $"cself[{ inputNumericNo }] - {Utils.WodiKs.ConvertNumericSpecialSettingTypeToName(specialSettingType)}";
            OutputStructTable outputStructTable = new OutputStructTable(entryName , header , data);

            return outputStructTable;
        }

        #endregion 数値入力の特殊設定

        #region 返り値

        private List<string> SetReturnHeader()
        {
            List<string> headerReturn = new List<string>() { "Name" , "Var" };
            return headerReturn;
        }

        private List<List<string>> SetReturnData(CommonEvent commonEvent)
        {
            List<List<string>> dataReturn = new List<List<string>>();
            int returnVar = commonEvent.Config.ReturnVariable;

            if (returnVar != -1)
            {
                var returnValueName = Utils.String.Trim(commonEvent.Config.ReturnValueName);
                var cselfVal = returnVar % 100;

                List<string> recordReturn = new List<string>() { returnValueName , $"\\cself[{ cselfVal.ToString() }]" };
                dataReturn.Add(recordReturn);
            }

            return dataReturn;
        }

        #endregion 返り値

        #region コモンセルフ変数

        private List<string> SetCSelfHeader()
        {
            List<string> headerCSelf = new List<string>() {
                    "Cself[0~19]" , "Name[0~19]" , "    ",
                    "Cself[20~39]" , "Name[20~39]" , "  　" ,
                    "Cself[40~59]" , "Name[40~59]" , "　  " ,
                    "Cself[60~79]" , "Name[60~79]" , "　　" ,
                    "Cself[80~99]" , "Name[80~99]" };           // DataTable型のColumsの名前は重複不可のため、空欄部分の文字列を少しずつ変更している
            return headerCSelf;
        }

        private List<List<string>> SetCSelfData(CommonEvent commonEvent)
        {
            string separator = "";
            int divideNum = commonEvent.CommonSelfNames.Length / 5;
            List<List<string>> cSelfData = new List<List<string>>();

            for (int i = 0; i < divideNum; i++)
            {
                List<string> recordCommonSelfNames = new List<string>() {
                    $"cself[{(divideNum * 0 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 0 + i]), separator ,
                    $"cself[{(divideNum * 1 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 1 + i]), separator ,
                    $"cself[{(divideNum * 2 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 2 + i]), separator ,
                    $"cself[{(divideNum * 3 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 3 + i]), separator ,
                    $"cself[{(divideNum * 4 + i).ToString() }]", Utils.String.Trim(commonEvent.CommonSelfNames[divideNum * 4 + i])};

                cSelfData.Add(recordCommonSelfNames);
            }

            return cSelfData;
        }

        #endregion コモンセルフ変数

        #region イベントコード & 動作指定コマンドコード
        private void SetEventCommandsAndMoveEventCommands(CommonEvent commonEvent)
        {
            EventCommandsStr eventCommandsStr = new EventCommandsStr();
            eventCommandsStr.SetEventCommandsAndMoveEventCommands(commonEvent.EventCommandList, (int)commonEvent.NumEventCommand);
            EventCommands = eventCommandsStr.EventCommands;
            MoveEventCommands = eventCommandsStr.MoveEventCommands;
        }
        #endregion イベントコード & 動作指定コマンドコード
    }
}