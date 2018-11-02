﻿using WodiKs.Ev.Common;

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
		/// <param name="triggerConditions"></param>
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
	}
}