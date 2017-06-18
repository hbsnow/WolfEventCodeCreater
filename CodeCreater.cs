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

                WriteFile(CommonEvent, Utils.File.format(commonName) + ".common.txt");
                count++;
            }

            return count + "件のコモンをテキストに変換しました。";
        }

        /// <summary>
        /// コモンイベントをファイルに書き込む
        /// </summary>
        /// <param name="CommonEvent"></param>
        /// <param name="filename"></param>
        private void WriteFile(CommonEvent CommonEvent, string filename)
        {
            List<string> eventCode = new List<string>();

            for (int i = 0; i < CommonEvent.NumEventCommand; i++)
            {
                EventCommand EventCommand = CommonEvent.EventCommandList[i];

                eventCode.Add(EventCommand.GetEventCode());
            }
            
            File.WriteAllLines(Dirpath + filename, eventCode.ToArray());
        }
    }
}
