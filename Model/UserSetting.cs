namespace WolfEventCodeCreater.Model
{
    public class UserSetting
    {
		/// <summary>
		/// ルートパス
		/// </summary>
		public string ProjectRoot = "";

		/// <summary>
		/// 出力するディレクトリ名
		/// </summary>
		public string OutputDirName = "Dump";

		/// <summary>
		/// コモンイベントの定義ファイル(dat)
		/// </summary>
		public string CommonEventPath = @"Data\BasicData";

		/// <summary>
		/// 可変DBの定義ファイル(project)
		/// </summary>
		public string CDBProjrctFilePath = @"Data\BasicData";

		/// <summary>
		/// 可変DBの定義ファイル(dat)
		/// </summary>
		public string CDBDatFilePath = @"Data\BasicData";

		/// <summary>
		/// ユーザーDBの定義ファイル(project)
		/// </summary>
		public string UDBProjrctFilePath = @"Data\BasicData";

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)
		/// </summary>
		public string UDBDatFilePath = @"Data\BasicData";

		/// <summary>
		/// システムDBの定義ファイル(project)
		/// </summary>
		public string SDBProjrctFilePath = @"Data\BasicData";

		/// <summary>
		/// システムDBの定義ファイル(dat)
		/// </summary>
		public string SDBDatFilePath = @"Data\BasicData";

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
