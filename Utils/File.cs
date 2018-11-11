using System.IO;
using System.Text;

namespace WolfEventCodeCreater.Utils
{
    public static class File
    {
        private static string SettingsFileName = "settings.xml";



        /// <summary>
        /// ユーザー設定を取得
        /// </summary>
        public static Model.UserSetting LoadUserSetting()
        {
            var settingFile = Path.Combine(Directory.GetCurrentDirectory(), SettingsFileName);
            
            // ユーザー設定ファイルがない場合にはデフォルト設定ファイルを出力
            if (!System.IO.File.Exists(settingFile))
            {
                var userSetting = new Model.UserSetting();
                WriteUserSetting(userSetting);

                return userSetting;
            }
            
            using (var streamReader = new StreamReader(settingFile, new UTF8Encoding(false)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Model.UserSetting));

                return (Model.UserSetting)serializer.Deserialize(streamReader);
            }
        }



        /// <summary>
        /// ユーザー設定の書き込み
        /// </summary>
        public static void WriteUserSetting(Model.UserSetting userSetting)
        {
            var settingFile = Path.Combine(Directory.GetCurrentDirectory(), SettingsFileName);

            using (var streamWriter = new StreamWriter(settingFile, false, new UTF8Encoding(false)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Model.UserSetting));
                serializer.Serialize(streamWriter, userSetting);
            }
        }



		/// <summary>
		/// ファイルの存在チェック
		/// </summary>
		/// <param name="appMesFileNameWhenNoFileExist">ファイルが存在しない場合にアプリメッセージへ追加するファイル名(""時は追加しない)</param>
		/// <returns>ファイルが存在する場合はtrue</returns>
		public static bool CheckFileExist(string filePath, string appMesFileNameWhenNoFileExist = "")
		{
			if (!System.IO.File.Exists(filePath))
			{
				if(appMesFileNameWhenNoFileExist != "")
				{
					AppMesOpp.AddAppMessge($"{appMesFileNameWhenNoFileExist} が\r\n" +
						$"{filePath} に見つかりません。");
				}
				System.Diagnostics.Debug.WriteLine($"{filePath} is No Exist.");
				return false;
			}
			return true;
		}



		/// <summary>
		/// ディレクトリの存在チェック
		/// </summary>
		/// <param name="appMesDirectoryNameWhenNoFileExist">ディレクトリが存在しない場合にアプリメッセージへ追加するディレクトリ名(""時は追加しない)</param>
		/// <param name="isMakeDirectoryWhenNoFileExist">ディレクトリが存在しない場合にディレクトリを作成するか</param>
		/// <returns>ファイルが存在する場合はtrue</returns>
		public static bool CheckDirectoryExist(string filePath , string appMesDirectoryNameWhenNoFileExist = "" , bool isMakeDirectoryWhenNoFileExist = false)
		{
			if (!Directory.Exists(filePath))
			{
				System.Diagnostics.Debug.WriteLine($"{filePath} is No Exist.");

				if (isMakeDirectoryWhenNoFileExist)
				{
					Directory.CreateDirectory(filePath);
					System.Diagnostics.Debug.WriteLine($"{filePath} is created newly because of No Exist.");
				}
				if (appMesDirectoryNameWhenNoFileExist != "")
				{
					if (isMakeDirectoryWhenNoFileExist)
					{
						AppMesOpp.AddAppMessge($"{appMesDirectoryNameWhenNoFileExist} を\r\n" +
						$"{filePath} に作成しました。");
					}
					else
					{
						AppMesOpp.AddAppMessge($"{appMesDirectoryNameWhenNoFileExist} が\r\n" +
						$"{filePath} に見つかりません。");
					}

					return false;
				}
			}

			return true;
		}
	}
}
