using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfEventCodeCreater.Utils
{
    public static class String
    {
        /// <summary>
        /// WodiKsで取得した文字列データのトリミング
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Trim(string val)
        {
            return val.TrimEnd('\0').Trim();
        }
    }
}
