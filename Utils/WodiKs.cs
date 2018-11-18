using WodiKs.Ev.Common;
using WodiKs.DB;

namespace WolfEventCodeCreater.Utils
{
	public static class WodiKs
	{
		/// <summary>
		/// CommonEventColor型を色名に変換する
		/// </summary>
		/// <param name="commonEventColor"></param>
		/// <returns></returns>
		public static string ConvertCommonEventColorToName(CommonEvent.CommonEventColor commonEventColor)
		{
			switch (commonEventColor)
			{
				case CommonEvent.CommonEventColor.Black:
					return "黒";

				case CommonEvent.CommonEventColor.Red:
					return "赤色";

				case CommonEvent.CommonEventColor.Blue:
					return "青色";

				case CommonEvent.CommonEventColor.Green:
					return "緑色";

				case CommonEvent.CommonEventColor.Purple:
					return "紫色";

				case CommonEvent.CommonEventColor.Yellow:
					return "黄色";

				case CommonEvent.CommonEventColor.Gray:
					return "グレー";

				default:
					return "";
			}
		}

		/// <summary>
		/// TriggerConditions型を起動条件名に変換する
		/// </summary>
		/// <param name="triggerConditions"></param>
		/// <returns></returns>
		public static string ConvertTriggerConditionsToName(CommonEvent.TriggerConditions triggerConditions)
		{
			switch (triggerConditions)
			{
				case CommonEvent.TriggerConditions.OnlyCall:
					return "呼び出しのみ";

				case CommonEvent.TriggerConditions.Automatic:
					return "自動実行";

				case CommonEvent.TriggerConditions.Parallel:
					return "並列実行";

				case CommonEvent.TriggerConditions.ParallelAlways:
					return "並列実行(常時)";

				default:
					return "";
			}
		}

		/// <summary>
		/// ComparisonMethod型を起動条件の比較方法名に変換する
		/// </summary>
		/// <param name="comparisonMethod"></param>
		/// <returns></returns>
		public static string ConvertComparisonMethodToName(CommonEvent.ComparisonMethod comparisonMethod)
		{
			switch (comparisonMethod)
			{
				case CommonEvent.ComparisonMethod.Greater:
					return "より大きい";

				case CommonEvent.ComparisonMethod.GreaterOrEqual:
					return "以上";

				case CommonEvent.ComparisonMethod.Same:
					return "と同じ";

				case CommonEvent.ComparisonMethod.Less:
					return "未満";

				case CommonEvent.ComparisonMethod.LessOrEqual:
					return "以下";

				case CommonEvent.ComparisonMethod.Not:
					return "以外";

				case CommonEvent.ComparisonMethod.SatisfyBitOf:
					return "とのビット積";

				default:
					return "";
			}
		}



		/// <summary>
		/// InputNumericData.SpecialSettingType型を数値入力の特殊設定方法名に変換する
		/// </summary>
		/// <param name="specialSettingType"></param>
		/// <returns></returns>
		public static string ConvertNumericSpecialSettingTypeToName(InputNumericData.SpecialSettingType specialSettingType)
		{
			switch (specialSettingType)
			{
				case InputNumericData.SpecialSettingType.NotUse:
					return "特殊な設定方法を使用しない";
				case InputNumericData.SpecialSettingType.ReferenceDatabase:
					return "データベース参照（数値）";
				case InputNumericData.SpecialSettingType.ManuallyGenerateBranch:
					return "選択肢を手動作成（数値）";
				default:
					return "";
			}
		}



		/// <summary>
		/// InputStringData.SpecialSettingType型を数値入力の特殊設定方法名に変換する
		/// </summary>
		/// <param name="specialSettingType"></param>
		/// <returns></returns>
		public static string ConvertStringSpecialSettingTypeToName(InputStringData.SpecialSettingType specialSettingType)
		{
			switch (specialSettingType)
			{
				case InputStringData.SpecialSettingType.NotUse:
					return "特殊な設定方法を使用しない";
				default:
					return "";
			}
		}



		/// <summary>
		/// DatabaseCategory型をデータベースの種類名に変換する
		/// </summary>
		/// <param name="dbCategory"></param>
		/// <returns></returns>
		public static string ConvertDatabaseCategoryToName(Database.DatabaseCategory dbCategory)
		{
			switch (dbCategory)
			{
				case Database.DatabaseCategory.Changeable:
					return "可変データベース";
				case Database.DatabaseCategory.User:
					return "ユーザーデータベース";
				case Database.DatabaseCategory.System:
					return "システムデータベース";
				case Database.DatabaseCategory.CommonEvent:
					return "コモンイベント";
				default:
					return "";
			}
		}



		/// <summary>
		/// SettingType型をデータIDの設定方法名に変換する
		/// </summary>
		/// <param name="settingType"></param>
		/// <returns></returns>
		public static string ConvertSettingTypeToName(TypeConfig.SettingType settingType)
		{
			switch (settingType)
			{
				case TypeConfig.SettingType.Manuall:
					return "手動で設定";
				case TypeConfig.SettingType.FirstStringData:
					return "最初の文字列データ";
				case TypeConfig.SettingType.PreviousTypeData:
					return "1つ前のタイプのデータID";
				case TypeConfig.SettingType.DesiredDBType:
					return "指定DBの指定タイプから";
				default:
					return "";
			}
		}



		/// <summary>
		/// ItemConfig.SpecialSettingType型を項目の特殊設定方法名に変換する
		/// </summary>
		/// <param name="specialSettingType"></param>
		/// <returns></returns>
		public static string ConvertItemConfigSpecialSettingTypeToName(ItemConfig.SpecialSettingType specialSettingType)
		{
			switch (specialSettingType)
			{
				case ItemConfig.SpecialSettingType.NotUse:
					return "特殊な設定方法を使用しない";
				case ItemConfig.SpecialSettingType.ReadFile:
					return "ファイル読み込み（文字列）";
				case ItemConfig.SpecialSettingType.ReferenceDatabase:
					return "データベース参照（数値）";
				case ItemConfig.SpecialSettingType.ManuallyGenerateBranch:
					return "選択肢を手動作成（数値）";
				default:
					return "";
			}
		}



		/// <summary>
		/// ItemConfig.ItemType型を項目内容のタイプ名に変換する
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static string ConvertItemTypeToName(ItemConfig.ItemType itemType)
		{
			switch (itemType)
			{
				case ItemConfig.ItemType.Numeric:
					return "数値";
				case ItemConfig.ItemType.String:
					return "文字列";
				default:
					return "";
			}
		}

	}
}