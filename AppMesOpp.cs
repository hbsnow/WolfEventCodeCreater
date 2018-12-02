using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WolfEventCodeCreater
{
	public static class AppMesOpp
	{
		private const string APP_MES_SEPARATOR = "------------------------------------";

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


		///<summary>現在のアプリメッセージの数</summary>
		public static int AppMesCount { get { return appMesList.Count; } }

		/// <summary>アプリのメッセージを追加設定する</summary>
		/// <param name="message">アプリのメッセージに追加する文字列</param>
		/// <param name="isError">エラー内容かどうか</param>
		/// <param name="isAddPrefixInformation">アプリのメッセージに接頭辞をつけるか</param>
		public static void AddAppMessge(string message, bool isError = false, bool isAddPrefixInformation = true)
		{
			string prefix = !isError ? "- " : "Error: ";
			prefix = isAddPrefixInformation ? prefix : "";
			appMesList.Add(prefix + message);
		}

		///<summary>
		///アプリの区切りメッセージを追加設定する
		///</summary>
		public static void AddSeparatorAppMessge()
		{
			appMesList.Add(APP_MES_SEPARATOR);
			AddAppMessgeBlank();
			AddAppMessgeBlank();
		}

		///<summary>アプリの空白メッセージを追加設定する</summary>
		public static void AddAppMessgeBlank()
		{
			appMesList.Add("");
		}

		///<summary>アプリの区切りメッセージを先頭と最後に設定する</summary>
		public static void AddEnclosedSeparatorAppMessge()
		{
			appMesList.Insert(0, APP_MES_SEPARATOR);
			appMesList.Add(APP_MES_SEPARATOR);
			AddAppMessgeBlank();
			AddAppMessgeBlank();
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