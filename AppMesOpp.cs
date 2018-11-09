using System.Collections.Generic;

namespace WolfEventCodeCreater
{
	public static class AppMesOpp
	{
		private static List<string> appMesList;
		private static string appMes;

		/// <summary>
		/// このアプリのメッセージ画面に出力する文字列
		/// </summary>
		public static string AppMes
		{
			get
			{
				appMes = "";
				appMesList.ForEach(x => appMes = appMes + x);
				return appMes;
			}
		}

		static AppMesOpp()
		{
			appMesList = new List<string>();
		}

		///<summary>
		///アプリのメッセージを追加設定する
		///</summary>
		public static void SetAppMessge(string message)
		{
			appMesList.Add(message);
		}
	}
}