using System.Collections.Generic;
using System.IO;
using WodiKs.Ev.Common;
using WodiKs.IO;

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

                MdList.Add($"# { commonName }\n");
                MdList.Add($"{ Utils.String.Trim(CommonEvent.Memo) }\n");

                if (CommonEvent.NumInputNumeric + CommonEvent.NumInputString > 0)
                {
                    MdList.Add("## 引数\n");
                    MdList.Add(" Type | Var | InitialValue | Name ");
                    MdList.Add("--- | --- | --- | --- ");
                    MdList = PushNumericConfig(MdList, CommonEvent);
                    MdList = PushStringConfig(MdList, CommonEvent);
                    MdList.Add("");
                }

                // {CommonEvent.Color.ToString()}

                // イベントコード
                MdList.Add("## イベントコード\n");
                MdList.Add("```");
                MdList = PushEventCode(MdList, CommonEvent);
                MdList.Add("```");

                File.WriteAllLines(filepath, MdList);

                count++;
            }

            return $"{ count }件のMarkdownを出力しました。";
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
                var initialValue = inputNumericData.InitialValue.ToString();
                var name = Utils.String.Trim(inputNumericData.Name);

                list.Add($"数値 | \\cself[{ i }] | { initialValue } | { name }");
            }

            return list;
        }



        /// <summary>
        /// 文字列の引数データをListに追加して戻す
        /// </summary>
        /// <param name="list"></param>
        /// <param name="CommonEvent"></param>
        /// <returns></returns>
        private List<string> PushStringConfig(List<string> list, CommonEvent commonEvent)
        {
            var commonEventConfig = commonEvent.Config;

            for (int i = 0; i < commonEvent.NumInputString; i++)
            {
                var inputStringData = commonEventConfig.InputStrings[i];
                var name = Utils.String.Trim(inputStringData.Name);

                list.Add($"文字列 | \\cself[{ i + 5 }] | | { name }");
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
