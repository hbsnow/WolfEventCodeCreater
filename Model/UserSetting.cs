namespace WolfEventCodeCreater.Model
{
    public class UserSetting
    {
        /// <summary>
        /// 出力するディレクトリ名
        /// </summary>
        public string OutputDirName = "Dump";

        /// <summary>
        /// 出力しないコモンのコメントアウト形式
        /// </summary>
        /// 
        public string CommentOut = "//";

        /// <summary>
        /// 出力のときファイル名にコモン番号をつけるかどうか
        /// </summary>
        public bool IsOutputCommonNumber = false;
    }
}
