using System.IO;
using System.Linq;

namespace WolfEventCodeCreater.Utils
{
    /// <summary>
    /// 文字列処理のユーティリティ
    /// </summary>
    public static class String
    {
        /// <summary>
        /// WodiKsで取得した文字列データのトリミング
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Trim(string val)
        {
            return val.TrimEnd('\0').Trim();
        }



        /// <summary>
        /// ファイル名に使用できない文字をフォーマットする
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string FormatFilename(string filename)
        {
            // ファイル名に使用できない文字列を'_'に変換
            char[] invaildChars = Path.GetInvalidFileNameChars();

            return invaildChars.Aggregate(
                filename, (s, c) => s.Replace(c.ToString(), "_")
            );
        }



		/// <summary>
		/// フラグ値を文字列に変換する
		/// </summary>
		/// <param name="isOnFlag"></param>
		/// <returns></returns>
		public static string ConvertFlagToString(bool isOnFlag)
		{
			return isOnFlag ? "On" : "Off";
		}
	}
}
