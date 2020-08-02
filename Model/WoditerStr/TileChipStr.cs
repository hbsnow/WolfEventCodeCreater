using WodiKs.Map.Tile;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
    public class TileChipStr
    {
        public TileSetStr Parent { get; private set; }
        public int Position_X { get; private set; }
        public int Position_Y { get; private set; }
        public OutputStructSentence PositionStr { get; private set; }
        public OutputStructSentence TagID { get; private set; }
        public OutputStructSentence PermissionFlag { get; private set; }
        //public OutputStructTable PermissionFlag { get; private set; }
        public OutputStructSentence PassableDirectionFlag { get; private set; }
        //public OutputStructTable PassableDirectionFlag { get; private set; }
        public OutputStructSentence CounterFlag { get; private set; }
        //public OutputStructTable CounterFlag { get; private set; }

        public TileChipStr(TileChip tileChip, int tileChipId, TileSetStr tileSetStr)
        {
            Parent = tileSetStr;
            Position_X = tileChipId % 8;
            Position_Y = tileChipId / 8;
            PositionStr = new OutputStructSentence("チップ位置", $"({Position_X.ToString()}, {Position_Y.ToString()})");
            TagID = new OutputStructSentence("チップタグID", Utils.String.Trim(tileChip.TagNumber.ToString()));
            PermissionFlag = new OutputStructSentence("通行許可設定", TileChipEnumHelper.DisplayName(tileChip.GetPermissionFlag()));
            //PermissionFlag = new OutputStructTable("通行許可設定", SetPermissionFlagHeader(), SetPermissionFlagData(tileChip.GetPermissionFlag()));
            PassableDirectionFlag = new OutputStructSentence("通行方向設定", TileChipEnumHelper.DisplayName(tileChip.GetPassableDirectionFlag()));
            //PassableDirectionFlag = new OutputStructTable("通行方向設定", SetPassableDirectionFlagHeader(), SetPassableDirectionFlagData(tileChip.GetPassableDirectionFlag()));
            CounterFlag = new OutputStructSentence("カウンター属性", Utils.String.ConvertFlagToString(tileChip.GetCounterFlag() == 0 ? false : true));
            //CounterFlag = new OutputStructTable("カウンター属性設定", SetCounterFlagHeader(), SetCounterFlagData(tileChip.GetCounterFlag()));
        }

        /*private List<string> SetPermissionFlagHeader()
        {
            return new List<string>() { "○", "×", "[左上]×", "[右上]×", "[左下]×", "[右下]×", "★", "▲", "↓", "□"};
        }

        private List<List<string>> SetPermissionFlagData(TileChip.PermissionFlags permissionFlags)
        {
            List<List<string>> data = new List<List<string>>() { };
            List<string> record = new List<string>();

            foreach (var permissionFlag in Enum.GetValues(typeof(TileChip.PermissionFlags)))
            {
                record.Add(Utils.String.ConvertFlagToString((0 < ((int)permissionFlags & (int)permissionFlag)) ? true : false));
            }
            data.Add(record);
            return data;
        }

        private List<string> SetPassableDirectionFlagHeader()
        {
            return new List<string>() { "↑", "→", "←", "↓"};
        }

        private List<List<string>> SetPassableDirectionFlagData(TileChip.PassableDirectionFlags passableDirectionFlags)
        {
            List<List<string>> data = new List<List<string>>() { };
            List<string> record = new List<string>();

            foreach (var passableDirectionFlag in Enum.GetValues(typeof(TileChip.PassableDirectionFlags)))
            {
                record.Add(Utils.String.ConvertFlagToString((0 < ((int)passableDirectionFlags & (int)passableDirectionFlag)) ? true : false));
            }
            data.Add(record);
            return data;
        }

        private List<string> SetCounterFlagHeader()
        {
            return new List<string>() { "カウンター属性" };
        }

        private List<List<string>> SetCounterFlagData(uint counterFlag)
        {
            List<List<string>> data = new List<List<string>>() { };

            data.Add(new List<string>() { Utils.String.ConvertFlagToString(counterFlag == 0 ? false : true)});
            return data;
        }*/
    }
}