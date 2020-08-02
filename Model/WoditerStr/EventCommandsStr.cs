using System.Collections.Generic;
using WodiKs.Ev;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
    public class EventCommandsStr
    {
        ///<summary>イベントコード</summary>
        public OutputStructSentences EventCommands { get; private set; }

        ///<summary>動作指定コマンドコード</summary>
        public List<OutputStructTables> MoveEventCommands { get; private set; }

        public EventCommandsStr()
        {
            MoveEventCommands = new List<OutputStructTables>();     // 事前にインスタンス生成が必要
        }

        public void SetEventCommandsAndMoveEventCommands(EventCommand[] eventCommands, int eventNum)
        {
            List<string> eventCommandList = new List<string>();
            List<OutputStructTables> moveEventCommandsList = new List<OutputStructTables>();

            eventCommandList.Add("WoditorEvCOMMAND_START");
            eventCommandList.AddRange(GetEventCodes(eventCommands, eventNum));
            eventCommandList.Add("WoditorEvCOMMAND_END");

            OutputStructSentences eventCommandsSentences = new OutputStructSentences("イベントコード", eventCommandList);

            EventCommands = eventCommandsSentences;
        }

        /// <summary>
        /// コモンイベントコードを取得する
        /// </summary>
        /// <param name="commonEvent"></param>
        /// <returns></returns>
        private List<string> GetEventCodes(EventCommand[] eventCommands, int eventNum)
        {
            List<string> eventCommandList = new List<string>();

            for (int i = 0; i < eventNum; i++)
            {
                var eventCommand = eventCommands[i];

                string eventCode = Utils.String.Trim(eventCommand.GetEventCode());

                // コモンイベントコードを出力用に最適化
                eventCode = Utils.String.RemoveDoubleCRCode(eventCode);
                eventCode = Utils.String.EncloseCRLFCodeOrSimpleLFCodeInLtAndGt(eventCode);

                eventCommandList.Add(eventCode);

                // 動作指定コマンドの場合、動作指定コマンドを取得
                if (eventCommand.IsMoveEvent)
                {
                    MoveEventCommands.Add(SetMoveEventTable(eventCommand, i + 1));
                }
            }

            return eventCommandList;
        }

        /// <summary>
        /// 動作指定コマンドコード関連情報を取得する
        /// </summary>
        /// <param name="eventCommand"></param>
        /// <param name="eventCommandLine"></param>
        /// <returns></returns>
        private OutputStructTables SetMoveEventTable(EventCommand eventCommand, int eventCommandLine)
        {
            List<OutputStructTable> moveEventCommandsTableList = new List<OutputStructTable>();

            // 動作指定機能フラグを取得しテーブル構造に整形
            moveEventCommandsTableList.Add(SetMoveEventFlag(eventCommand.MoveEventFlag));

            // 動作指定コマンドコードをテーブル構造に整形
            moveEventCommandsTableList.Add(SetMoveEventCommandsTable(eventCommand.MoveEventCommandList, "動作指定コマンドコード"));

            OutputStructTables moveEventCommandsTables = new OutputStructTables($"{ eventCommandLine.ToString() }行目(イベントコード)", moveEventCommandsTableList);

            return moveEventCommandsTables;
        }

        ///<summary>動作指定機能フラグを取得しテーブル構造化</summary>
        private OutputStructTable SetMoveEventFlag(byte moveEventFlag)
        {
            List<string> headerMoveEventFlag = new List<string>() { "動作完了までウェイト", "動作を繰り返す", "移動できない場合は飛ばす" };
            List<List<string>> dataMoveEventFlag = new List<List<string>>() {new List<string>(){
                            Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.WaitForFinish)) ? true : false),
                            Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.RepeatMovement)) ? true : false),
                            Utils.String.ConvertFlagToString((0 < (moveEventFlag & (byte)MoveEventFlags.SkipImpossibleMoves)) ? true : false)} };

            OutputStructTable moveEventFlagTable = new OutputStructTable("動作指定機能フラグ", headerMoveEventFlag, dataMoveEventFlag);

            return moveEventFlagTable;
        }

        ///<summary>動作指定コマンドコードを取得しテーブル構造化</summary>
        private OutputStructTable SetMoveEventCommandsTable(MoveEventCommand[] moveEventCommands, string tableName)
        {
            int maxNumNumericData = 0;          // あるイベントコードにおける全ての動作指定コマンドコードの最大数値データ数
            maxNumNumericData = GetMaxNumNumericDataOfMoveEventCommandCode(moveEventCommands);
            List<string> moveEventCommandsHeader = SetMoveEventCommandsHeader(ref maxNumNumericData);
            List<List<string>> moveEventCommandsData = SetMoveEventCommandsData(moveEventCommands, maxNumNumericData);

            OutputStructTable moveEventCommandsTable = new OutputStructTable(tableName, moveEventCommandsHeader, moveEventCommandsData);

            return moveEventCommandsTable;
        }

        /// <summary>
        /// 動作指定コマンドコードの数値データの最大データ数を取得する
        /// </summary>
        /// <param name="eventCommand"></param>
        /// <returns></returns>
        private int GetMaxNumNumericDataOfMoveEventCommandCode(MoveEventCommand[] moveEventCommands)
        {
            int maxNumNumericData = 0;

            foreach (MoveEventCommand moveEventCommand in moveEventCommands)
            {
                if (maxNumNumericData < moveEventCommand.NumNumericData)
                {
                    maxNumNumericData = moveEventCommand.NumNumericData;
                }
            }
            return maxNumNumericData;
        }

        private List<string> SetMoveEventCommandsHeader(ref int maxNumNumericData)
        {
            List<string> headerMoveEventCommands = new List<string>() { "動作ID" };

            if (0 < maxNumNumericData)
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

            return headerMoveEventCommands;
        }

        private List<List<string>> SetMoveEventCommandsData(MoveEventCommand[] moveEventCommands, int maxNumNumericData)
        {
            List<List<string>> moveEventCommandsData = new List<List<string>>();

            foreach (MoveEventCommand moveEventCommand in moveEventCommands)
            {
                List<string> recordMoveEventCommandCode = new List<string>() { };

                recordMoveEventCommandCode.Add(moveEventCommand.MoveCommandID.ToString());

                foreach (int numData in moveEventCommand.NumericList)
                {
                    recordMoveEventCommandCode.Add(numData.ToString());
                }

                // 最大数値データ数より足りない分を""で埋める
                int lackNumNumericData = maxNumNumericData - moveEventCommand.NumNumericData;
                for (int i = 0; i < lackNumNumericData; i++)
                {
                    recordMoveEventCommandCode.Add("");
                }

                moveEventCommandsData.Add(recordMoveEventCommandCode);
            }

            return moveEventCommandsData;
        }

        ///<summary>MapEventの動作指定機能フラグを取得</summary>
        public OutputStructTable SetMoveEventFlagForMapEvent(byte moveEventFlag)
        {
            return SetMoveEventFlag(moveEventFlag);
        }

        ///<summary>MapEventの動作指定コマンドリストを取得</summary>
        public OutputStructTable SetMoveEventCommandsTableForMapEvent(MoveEventCommand[] moveEventCommands, string tableName)
        {
            return SetMoveEventCommandsTable(moveEventCommands, tableName);
        }
    }
}