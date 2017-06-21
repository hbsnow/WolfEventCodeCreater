using System;
using System.Collections.Generic;
using System.IO;
using WodiKs.Ev;
using WodiKs.Ev.Common;
using WodiKs.IO;

namespace WolfEventCodeCreater
{
    /// <summary>
    /// Markdown生成
    /// </summary>
    public class CodeCreater
    {
        private Config Config;
        private CommonEventManager CommonEventManager;
        private List<string> MdList;



        public CodeCreater(Config config, CommonEventDatReader commonEventDatReader)
        {
            Config = config;
            CommonEventManager = commonEventDatReader.GetReadData();
        }



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

            int count = 0;
            for (int i = 0; i < CommonEventManager.NumCommonEvent; i++)
            {
                var CommonEvent = CommonEventManager.CommonEvents[i];

                // コモン名のトリミング
                string commonName = Utils.String.Trim(CommonEvent.CommonEventName);

                // コマンド数2未満、あるいはコモン名の入力がないもの、コメントアウトのものは除外
                if (CommonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf("//") == 1)
                {
                    continue;
                }

                string filepath = Path.Combine(Config.DumpDir, $"{ Utils.String.FormatFilename(commonName) }.common.md");

                MdList = new List<string>();

                MdList.Add($"# {commonName}\n");
                MdList.Add($"{Utils.String.Trim(CommonEvent.Memo)}\n");

                MdList.Add("## 引数\n");
                MdList.Add(" No | InitialValue | Name ");
                MdList.Add(" --- | --- | --- ");
                var config = CommonEvent.Config;
                // このへんはメソッドにする
                MdList.Add($"\\cself[0] | {Utils.String.Trim(CommonEvent.CommonSelfNames[0])}");
                MdList.Add($"\\cself[1] | {Utils.String.Trim(CommonEvent.CommonSelfNames[1])}");
                MdList.Add($"\\cself[2] | {Utils.String.Trim(CommonEvent.CommonSelfNames[2])}");
                MdList.Add($"\\cself[3] | {Utils.String.Trim(CommonEvent.CommonSelfNames[3])}");
                MdList.Add($"\\cself[5] | {Utils.String.Trim(CommonEvent.CommonSelfNames[5])}");
                MdList.Add($"\\cself[6] | {Utils.String.Trim(CommonEvent.CommonSelfNames[6])}");
                MdList.Add($"\\cself[7] | {Utils.String.Trim(CommonEvent.CommonSelfNames[7])}");
                MdList.Add($"\\cself[8] | {Utils.String.Trim(CommonEvent.CommonSelfNames[8])}\n");


                // {CommonEvent.Color.ToString()}
                Console.WriteLine(CommonEvent.NumInputNumeric);
                Console.WriteLine(CommonEvent.NumInputString);


                // イベントコード
                MdList.Add("```");
                MdList = PushEventCode(MdList, CommonEvent);
                MdList.Add("```");

                File.WriteAllLines(filepath, MdList);

                count++;
            }

            return $"{count}件のMarkdownを出力しました。";
        }



        /// <summary>
        /// 数値の引数データをListに追加して戻す
        /// </summary>
        /// <param name="list"></param>
        /// <param name="CommonEvent"></param>
        /// <returns></returns>
        private List<string> PushNumericConfig(List<string> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputNumeric; i++)
            {
                var inputNumericData =  commonEventConfig.InputNumerics[i];

                list.Add(inputNumericData.InitialValue.ToString());
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
