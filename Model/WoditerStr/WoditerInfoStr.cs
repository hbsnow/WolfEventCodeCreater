using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WodiKs.DB;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class WoditerInfoStr
	{
		public WoditerInfo Source { get; private set;}
		private Config config;
		
		///<summary>文字列化したCommonEvent情報</summary>
		//public List<CommonEvStr> CEvStrs{ get; private set; }

		///<summary>文字列化したCDB情報</summary>
		public List<DatabaseTypeStr> CDBStrs { get; private set; }

		///<summary>文字列化したUDB情報</summary>
		public List<DatabaseTypeStr> UDBStrs { get; private set; }
		
		///<summary>文字列化したSDB情報</summary>
		public List<DatabaseTypeStr> SDBStrs { get; private set; }

		public WoditerInfoStr(WoditerInfo woditerInfo, Config config)
		{
			if (woditerInfo == null)
				return;

			Source = woditerInfo;
			this.config = config;
			
			/*if (woditerInfo.CEvMgr != null)
			{

			}*/

			if (woditerInfo.CDB != null)
			{
				CDBStrs = SetDBTypeStrs(woditerInfo.CDB, Database.DatabaseCategory.Changeable);
			}

			if (woditerInfo.UDB != null)
			{
				UDBStrs = SetDBTypeStrs(woditerInfo.UDB, Database.DatabaseCategory.User);
			}

			if (woditerInfo.SDB != null)
			{
				SDBStrs = SetDBTypeStrs(woditerInfo.SDB, Database.DatabaseCategory.System);
			}
		}

		//private List<CommonEvStr> SetCEvStrs() { }

		private List<DatabaseTypeStr> SetDBTypeStrs(Database db, Database.DatabaseCategory databaseCategory)
		{
			List<DatabaseTypeStr> dBStrs = new List<DatabaseTypeStr>();

			for(int typeIDNo = 0; typeIDNo < db.NumType; typeIDNo++)
			{
				// データ数0、各項目設定データが0、タイプ名の入力がないもの、コメントアウトのものは除外
				string typeName = Utils.String.Trim(db.TypesData[typeIDNo].TypeName);
				if (db.TypesData[typeIDNo].NumData == 0 || typeName == "" || typeName.IndexOf(config.CommentOut) == 0)
				{
					continue;
				}

				dBStrs.Add(new DatabaseTypeStr(db.TypesData[typeIDNo] , typeIDNo, databaseCategory, Source));
			}
			return dBStrs;
		}

	}
}
