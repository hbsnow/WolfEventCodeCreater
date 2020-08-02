using System.Collections.Generic;
using System.Data;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.StrFormat
{
    /// <summary>
    /// 出力ファイル用に文字列を整形するための抽象クラス
    /// </summary>
    public abstract class StrFormatBase
    {
        protected string columnDelimiter;         // 列同士の区切り文字
        protected string betweenHeaderAndDataDelimiter;           // ヘッダ部とデータ部の区切り文字
        protected string vBarRuledLine;      // 垂直文字の罫線
        protected string branchRuledLine;        // 分岐文字の罫線
        protected string branchWithLastItemRuledLine;       // グループ内最後尾項目の分岐文字の罫線

        /// <summary>
        /// 見出しの文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="inputStr">整形対象の文字列</param>
        /// <param name="headlineLevel">見出しのレベル(既定値は2)</param>
        /// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        public abstract List<string> FormatHeadline(List<string> mdList , string inputStr , int headlineLevel = 2, bool isAddLFCodeInLastStr = true);

        /// <summary>
        /// 単文の文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="inputStr">整形対象の文字列</param>
        /// <param name="isAddLFCodeInLastStr">文の最後にLFコードを付け足すか</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        public abstract List<string> FormatSimpleSentence(List<string> mdList , string inputStr, bool isAddLFCodeInLastStr = true);

        /// <summary>
        /// コード文の文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="inputStrs">整形対象の文字列リスト</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        public abstract List<string> FormatCode(List<string> mdList, List<string> inputStrs);

        /// <summary>
        /// テーブル構造（ヘッダ部とデータ部とフッタ部）を整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="outputStructTable">出力元のテーブル構造</param>
        /// <param name="maxRowNum">テーブルのデータのうち1列に格納する最大行数</param>
        /// <param name="isSimpleSentenceWhenOnlyOneColumn">データが一列のみのときに文章に変更するかどうか</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        public abstract List<string> FormatTable(List<string> mdList, OutputStructTable outputStructTable,
            int maxRowNum = 20, bool isSimpleSentenceWhenOnlyOneColumn = true);

        /// <summary>
        /// テーブルのヘッダ部の文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="outputStructTable">出力元のテーブル構造</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        protected abstract List<string> FormatTableHeader(List<string> mdList , OutputStructTable outputStructTable);

        /// <summary>
        /// テーブルのデータ部の文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="outputStructTable">出力元のテーブル構造</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        protected abstract List<string> FormatTableData(List<string> mdList , OutputStructTable outputStructTable);

        /// <summary>
        /// テーブルのフッタ部の文字列に整形する
        /// </summary>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="inputStr">整形対象の文字列</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        protected abstract List<string> FormatTableFooter(List<string> mdList , string inputStr);

        /// <summary>
        /// ツリー構造のノードを整形する
        /// </summary>
        /// <typeparam name="T">クラス</typeparam>
        /// <param name="mdList">出力文字列が格納されたリスト</param>
        /// <param name="outputStructTree">出力元のTree構造</param>
        /// <returns>整形済みの文字列が入力された出力文字列リスト</returns>
        public abstract List<string> FormatTree<T>(List<string> mdList, OutputStructTree<T> outputStructTree) where T : class;
    }
}