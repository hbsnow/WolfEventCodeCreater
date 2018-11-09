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
		///<summary>文字列化したCommonEvent情報</summary>
		//public List<CommonEvStr> CEvStrs{ get; private set; }

		///<summary>文字列化したCDB情報</summary>
		public List<DatabaseTypeStr> CDBStrs { get; private set; }

		///<summary>文字列化したUDB情報</summary>
		public List<DatabaseTypeStr> UDBStrs { get; private set; }
		
		///<summary>文字列化したSDB情報</summary>
		public List<DatabaseTypeStr> SDBStrs { get; private set; }

		public WoditerInfoStr(WoditerInfo woditerInfo)
		{
			if (woditerInfo == null)
				return;

			/*if (woditerInfo.CEvMgr != null)
			{

			}*/

			if (woditerInfo.CDB != null)
			{
				CDBStrs = SetDBStrs(woditerInfo.CDB);
			}

			if (woditerInfo.UDB != null)
			{
				UDBStrs = SetDBStrs(woditerInfo.UDB);
			}

			if (woditerInfo.SDB != null)
			{
				SDBStrs = SetDBStrs(woditerInfo.SDB);
			}
		}

		//private List<CommonEvStr> SetCEvStrs() { }

		private List<DatabaseTypeStr> SetDBStrs(Database db)
		{
			List<DatabaseTypeStr> dBStrs = new List<DatabaseTypeStr>();

			for(int typeIDNo = 0; typeIDNo < db.NumType; typeIDNo++)
			{
				dBStrs.Add(new DatabaseTypeStr(db.TypesData[typeIDNo] , typeIDNo));
			}
			return dBStrs;
		}

	}
}
