using System;
using System.Collections.Generic;
using System.IO;
using WodiKs.Ev;
using WodiKs.Ev.Common;
using WodiKs.IO;

namespace WolfEventCodeCreater
{
    public class CodeCreater
    {
        /// <summary>
        /// プロジェクトのルートパス
        /// </summary>
        public string Rootpath { get; set; }

        /// <summary>
        /// ファイル出力するディレクトリのパス
        /// </summary>
        private string Dirpath { get; set; }


        public string Create()
        {
            var commonFilepath = Rootpath + @"\Data\BasicData\CommonEvent.dat";
            var CommonEventReader = new CommonEventDatReader(commonFilepath);
            var CommonEventManager = CommonEventReader.GetReadData();

            if (CommonEventManager == null)
            {
                return "ファイルがみつからない、あるいは使用中のため出力に失敗しました。";
            }

            // ファイル出力するディレクトリの作成
            Dirpath = Path.Combine(Rootpath, "Dump\\");

            if (!Directory.Exists(Dirpath))
            {
                Directory.CreateDirectory(Dirpath);
            }

            int count = 0;
            for (int i = 0; i < CommonEventManager.NumCommonEvent; i++)
            {
                CommonEvent CommonEvent = CommonEventManager.CommonEvents[i];

                // コモン名のトリミング
                String commonName = Utils.String.Trim(CommonEvent.CommonEventName);

                // コマンド数2未満、あるいはコモン名の入力がないもの、コメントアウトのものは除外
                if (CommonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf("//") == 1)
                {
                    continue;
                }

                string filepath = Dirpath + Utils.File.Format(commonName) + ".common.md";
                List<string> mdList = new List<string>();

                mdList.Add($"# {commonName}\n");

                mdList.Add($"{Utils.String.Trim(CommonEvent.Memo)}\n");

                mdList.Add("## 引数\n");

                mdList.Add("引数   | セルフ変数名");
                mdList.Add(" ----- | ----- ");
                // このへんはメソッドにする
                mdList.Add($"\\cself[0] | {Utils.String.Trim(CommonEvent.CommonSelfNames[0])}");
                mdList.Add($"\\cself[1] | {Utils.String.Trim(CommonEvent.CommonSelfNames[1])}");
                mdList.Add($"\\cself[2] | {Utils.String.Trim(CommonEvent.CommonSelfNames[2])}");
                mdList.Add($"\\cself[3] | {Utils.String.Trim(CommonEvent.CommonSelfNames[3])}");
                mdList.Add($"\\cself[5] | {Utils.String.Trim(CommonEvent.CommonSelfNames[5])}");
                mdList.Add($"\\cself[6] | {Utils.String.Trim(CommonEvent.CommonSelfNames[6])}");
                mdList.Add($"\\cself[7] | {Utils.String.Trim(CommonEvent.CommonSelfNames[7])}");
                mdList.Add($"\\cself[8] | {Utils.String.Trim(CommonEvent.CommonSelfNames[8])}\n");


                // {CommonEvent.Color.ToString()}


                // イベントコード
                mdList.Add("\n```");
                mdList = PushEventCode(mdList, CommonEvent);
                mdList.Add("```");

                File.WriteAllLines(filepath, mdList);

                count++;
            }

            return $"{count}件のコモンをMarkdownに変換しました。";
        }

        /// <summary>
        /// コモンイベントをListに追加する
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="CommonEvent"></param>
        /// <returns></returns>
        private List<string> PushEventCode(List<string> eventCode, CommonEvent CommonEvent)
        {
            eventCode.Add("WoditorEvCOMMAND_START");

            for (int i = 0; i < CommonEvent.NumEventCommand; i++)
            {
                EventCommand EventCommand = CommonEvent.EventCommandList[i];

                eventCode.Add(EventCommand.GetEventCode());
            }

            eventCode.Add("WoditorEvCOMMAND_END");

            return eventCode;
        }
    }
}
