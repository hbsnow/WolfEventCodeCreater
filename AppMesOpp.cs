using System.Collections.Generic;
using System.Windows.Forms;

namespace WolfEventCodeCreater
{
	public static class AppMesOpp
	{
		private const string APP_MES_SEPARATOR = "------------------------------------\r\n\r\n";

		private static List<string> appMesList;

		static AppMesOpp()
		{
			appMesList = new List<string>();
		}

		///<summary>セパレーター</summary>
		public static string AppMesSeparator
		{
			get { return APP_MES_SEPARATOR; }
		}

		///<summary>
		///アプリのメッセージを追加設定する
		///</summary>
		public static void AddAppMessge(string message)
		{
			appMesList.Add(message);
		}

		///<summary>
		///アプリの区切りメッセージを追加設定する
		///</summary>
		public static void AddSeparatorAppMessge()
		{
			appMesList.Add(APP_MES_SEPARATOR);
		}

		///<summary>
		///アプリのメッセージを返す
		///</summary>
		public static string ReturnAppMessge(bool isClearAppMessList = true)
		{
			string appMes = "";
			appMesList.ForEach(x => appMes = appMes + "\r\n" + x);

			if (isClearAppMessList)
			{
				appMesList.Clear();
			}
			return appMes;
		}

		///<summary>
		///アプリのメッセージをクリアする
		///</summary>
		public static void ClearAppMessge()
		{
			appMesList.Clear();
		}
	}
}