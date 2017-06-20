using System.Linq;

namespace WolfEventCodeCreater.Utils
{
    public static class File
    {
        /// <summary>
        /// ファイル名に使用できない文字をフォーマットする
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Format(string filename)
        {
            // ファイル名に使用できない文字列を'_'に変換
            char[] invaildChars = new char[]
            {
                    '\\', '/', ':', ',', ';', '*', '?', '<', '>', '|', ' '
            };

            return invaildChars.Aggregate(
                filename, (s, c) => s.Replace(c.ToString(), "_")
            );
        }
    }
}
