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
                String commonName = CommonEvent.CommonEventName.TrimEnd('\0').Trim();

                // コマンド数2未満、あるいはコモン名の入力がないもの、コメントアウトのものは除外
                if (CommonEvent.NumEventCommand < 2 || commonName == "" || commonName.IndexOf("//") == 1)
                {
                    continue;
                }

                string filepath = Dirpath + Utils.File.format(commonName) + ".common.md";
                List<string> mdList = new List<string>();

                // イベントコード
                mdList.Add("```");
                mdList = PushEventCode(mdList, CommonEvent);
                mdList.Add("```");

                File.WriteAllLines(filepath, mdList);

                count++;
            }

            return count + "件のコモンをテキストに変換しました。";
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
