using WodiKs.Ev.Common;
using WodiKs.DB;
using WodiKs.Map.Ev;

namespace WolfEventCodeCreater.Utils
{
	public static class WodiKs
	{
		///<summary>DB参照時などにデータ取得できない場合の値</summary>
		public const string NO_DATA = "☓ NoData";

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
		/// CommonEvent.ComparisonMethod型を起動条件の比較方法名に変換する
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

				case CommonEvent.ComparisonMethod.LessOrEqual:
					return "以下";

				case CommonEvent.ComparisonMethod.Less:
					return "未満";

				case CommonEvent.ComparisonMethod.Not:
					return "以外";

				case CommonEvent.ComparisonMethod.SatisfyBitOf:
					return "とのビット積";

				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.ComparisonMethod型を起動条件の比較方法名に変換する
		/// </summary>
		/// <param name="comparisonMethod"></param>
		/// <returns></returns>
		public static string ConvertComparisonMethodToName(MapEventPage.ComparisonMethod comparisonMethod)
		{
			switch (comparisonMethod)
			{
				case MapEventPage.ComparisonMethod.Greater:
					return "より大きい";

				case MapEventPage.ComparisonMethod.GreaterOrEqual:
					return "以上";

				case MapEventPage.ComparisonMethod.Same:
					return "と同じ";

				case MapEventPage.ComparisonMethod.LessOrEqual:
					return "以下";

				case MapEventPage.ComparisonMethod.Less:
					return "未満";

				case MapEventPage.ComparisonMethod.Not:
					return "以外";

				case MapEventPage.ComparisonMethod.SatisfyBitOf:
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



		/// <summary>
		/// MapEventPage.GraphicTypes型をグラフィックの設定タイプ名に変換する
		/// </summary>
		/// <param name="graphicType"></param>
		/// <returns></returns>
		public static string ConvertGraphicTypesToName(MapEventPage.GraphicTypes graphicType)
		{
			switch (graphicType)
			{
				case MapEventPage.GraphicTypes.NoGraphic:
					return "グラフィック無し";
				case MapEventPage.GraphicTypes.CharaChip:
					return "キャラチップ";
				case MapEventPage.GraphicTypes.TileSet:
					return "タイルセット";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.TriggerConditions型をマップイベント起動条件名に変換する
		/// </summary>
		/// <param name="triggerConditionsType"></param>
		/// <returns></returns>
		public static string ConvertTriggerConditionsToName(MapEventPage.TriggerConditions triggerConditionsType)
		{
			switch (triggerConditionsType)
			{
				case MapEventPage.TriggerConditions.ByEnter:
					return "決定キーで実行";
				case MapEventPage.TriggerConditions.Automatic:
					return "自動実行";
				case MapEventPage.TriggerConditions.Parallel:
					return "並列実行";
				case MapEventPage.TriggerConditions.PlayerContact:
					return "プレイヤー接触";
				case MapEventPage.TriggerConditions.EnterContact:
					return "イベント接触";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.MoveTypes型を移動タイプ名に変換する
		/// </summary>
		/// <param name="moveTypes"></param>
		/// <returns></returns>
		public static string ConvertMoveTypesToName(MapEventPage.MoveTypes moveTypes)
		{
			switch (moveTypes)
			{
				case MapEventPage.MoveTypes.NoMove:
					return "動かない";
				case MapEventPage.MoveTypes.Custom:
					return "カスタム";
				case MapEventPage.MoveTypes.Random:
					return "ランダム";
				case MapEventPage.MoveTypes.MoveToPlayer:
					return "ﾌﾟﾚｰﾔｰ接近";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.MoveSpeedTypes型を移動速度名に変換する
		/// </summary>
		/// <param name="moveSpeedTypes"></param>
		/// <returns></returns>
		public static string ConvertMoveSpeedTypesToName(MapEventPage.MoveSpeedTypes moveSpeedTypes)
		{
			switch (moveSpeedTypes)
			{
				case MapEventPage.MoveSpeedTypes._0_Slowest:
					return "最遅";
				case MapEventPage.MoveSpeedTypes._1_:
					return "遅い";
				case MapEventPage.MoveSpeedTypes._2_:
					return "少し遅い";
				case MapEventPage.MoveSpeedTypes._3_Normal:
					return "標準";
				case MapEventPage.MoveSpeedTypes._4_:
					return "少し速い";
				case MapEventPage.MoveSpeedTypes._5_:
					return "速い";
				case MapEventPage.MoveSpeedTypes._6_Fastest:
					return "最速";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.MoveFrequencyTypes型を移動頻度名に変換する
		/// </summary>
		/// <param name="moveFrequencyType"></param>
		/// <returns></returns>
		public static string ConvertMoveFrequencyTypesToName(MapEventPage.MoveFrequencyTypes moveFrequencyTypes)
		{
			switch (moveFrequencyTypes)
			{
				case MapEventPage.MoveFrequencyTypes.EveryFrame:
					return "毎フレーム";
				case MapEventPage.MoveFrequencyTypes.VeryFrequent:
					return "超短間隔";
				case MapEventPage.MoveFrequencyTypes.Frequent:
					return "短間隔";
				case MapEventPage.MoveFrequencyTypes.Normal:
					return "中間隔";
				case MapEventPage.MoveFrequencyTypes.Infrequent:
					return "大間隔";
				case MapEventPage.MoveFrequencyTypes.Seldom:
					return "超大間隔";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.AnimationSpeedTypes型をアニメ速度名に変換する
		/// </summary>
		/// <param name="animationSpeedTypes"></param>
		/// <returns></returns>
		public static string ConvertAnimationSpeedTypesToName(MapEventPage.AnimationSpeedTypes animationSpeedTypes)
		{
			switch (animationSpeedTypes)
			{
				case MapEventPage.AnimationSpeedTypes.EveryFrame:
					return "毎フレーム";
				case MapEventPage.AnimationSpeedTypes.VeryFrequent:
					return "超短間隔";
				case MapEventPage.AnimationSpeedTypes.Frequent:
					return "短間隔";
				case MapEventPage.AnimationSpeedTypes.Normal:
					return "中間隔";
				case MapEventPage.AnimationSpeedTypes.Infrequent:
					return "大間隔";
				case MapEventPage.AnimationSpeedTypes.Seldom:
					return "超大間隔";
				default:
					return "";
			}
		}



		/// <summary>
		/// MapEventPage.MoveOptionFlags型を移動オプション名に変換する
		/// </summary>
		/// <param name="moveOptionFlags"></param>
		/// <returns></returns>
		public static string ConvertMoveOptionFlagsToName(MapEventPage.MoveOptionFlags moveOptionFlags)
		{
			switch (moveOptionFlags)
			{
				case MapEventPage.MoveOptionFlags.StandbyAnimation:
					return "待機時アニメ";
				case MapEventPage.MoveOptionFlags.MovementAnimation:
					return "移動時アニメ";
				case MapEventPage.MoveOptionFlags.LockDirection:
					return "方向固定";
				case MapEventPage.MoveOptionFlags.PassThrough:
					return "すり抜け";
				case MapEventPage.MoveOptionFlags.AboveThePlayer:
					return "主人公より上";
				case MapEventPage.MoveOptionFlags.CollisionDetection:
					return "当ﾀﾘ判定■";
				case MapEventPage.MoveOptionFlags.ArrangeHalfCellHigher:
					return "半歩上に設置";
				default:
					return "";
			}
		}
	}
}