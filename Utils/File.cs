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
    }
}
