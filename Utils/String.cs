using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
		/// <para>二つの連続したCRコード(\r\r)を文字列から取り除く。</para>
		/// <para>WodiKsで取得したコモンイベントコードを外部ファイル出力した際に</para>
		/// <para>CRコードによって文字列が複数行に記述される問題を解決する。</para>
		/// <para>※最終的に出力される文字列はウディタへ「ｸﾘｯﾌﾟﾎﾞｰﾄﾞ→ｺｰﾄﾞ貼り付け」可能となる</para>
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string RemoveDoubleCRCode(string val)
		{
			return val.Replace("\r\r","");
		}



		/// <summary>
		/// <para>改行文字(\r\n)またはLFコード(\n)単体を&lt;\n&gt;に置換する。</para>
		/// <para>WodiKsで取得したコモンイベントコードを外部ファイル出力した際に</para>
		/// <para>改行文字やLFコードによって文字列が複数行に記述される問題、及び、</para>
		/// <para>WodiKsで取得(又はクリップボードコピー)されたイベントコードが改行を意図とした改行文字やLFコードを含んでいた場合に</para>
		/// <para>ウディタへクリップボード貼り付けできないウディタ仕様の問題を解決する。</para>
		/// <para>※最終的に出力される文字列はウディタへ「ｸﾘｯﾌﾟﾎﾞｰﾄﾞ→ｺｰﾄﾞ貼り付け」可能となる</para>
		/// <para>※但し、「ｲﾍﾞﾝﾄｺｰﾄﾞ→ｸﾘｯﾌﾟﾎﾞｰﾄﾞへｺﾋﾟｰ」時に実際にコピーされる文字列は&lt;\n&gt;を含んでいない</para>
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string EncloseCRLFCodeOrSimpleLFCodeInLtAndGt(string val)
		{
			val = Regex.Replace(val, "(?<![\r])\n" , "<\\n>");          // 否定的後読みの正規表現
			return  val.Replace("\r\n" , "<\\n>");
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
