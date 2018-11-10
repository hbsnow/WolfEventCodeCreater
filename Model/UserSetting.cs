namespace WolfEventCodeCreater.Model
{
    public class UserSetting
    {
        /// <summary>
        /// 出力するディレクトリ名
        /// </summary>
        public string OutputDirName = "Dump";

		/// <summary>
		/// コモンイベントの定義ファイル(dat)
		/// </summary>
		public string CommonEventPath = @"Data\BasicData\CommonEvent.dat";

		/// <summary>
		/// 可変DBの定義ファイル(project)
		/// </summary>
		public string CDBProjrctFilePath = @"Data\BasicData\CDataBase.project";

		/// <summary>
		/// 可変DBの定義ファイル(dat)
		/// </summary>
		public string CDBDatFilePath = @"Data\BasicData\CDataBase.dat";

		/// <summary>
		/// ユーザーDBの定義ファイル(project)
		/// </summary>
		public string UDBProjrctFilePath = @"Data\BasicData\UDataBase.project";

		/// <summary>
		/// ユーザーDBの定義ファイル(dat)
		/// </summary>
		public string UDBDatFilePath = @"\Data\BasicData\UDataBase.dat";

		/// <summary>
		/// システムDBの定義ファイル(project)
		/// </summary>
		public string SDBProjrctFilePath = @"Data\BasicData\SDataBase.project";

		/// <summary>
		/// システムDBの定義ファイル(dat)
		/// </summary>
		public string SDBDatFilePath = @"\Data\BasicData\SDataBase.dat";

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
