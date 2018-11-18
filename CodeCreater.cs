using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WodiKs.Ev.Common;
using WodiKs.IO;
using WolfEventCodeCreater.StrFormat;
using WodiKs.Ev;
using WolfEventCodeCreater.Model;

namespace WolfEventCodeCreater
{
    /// <summary>
    /// Markdown生成
    /// </summary>
    public class CodeCreater
    {
        private Model.Config Config;

        public CodeCreater(Model.Config config)
        {
            Config = config;
        }



        /// <summary>
        /// Markdownファイルの出力
        /// </summary>
        /// <returns></returns>
        public void Write()
        {
			// ウディタ情報を取得
			OutputDriver outputDriver = new OutputDriver(Config);

			// ファイル出力処理
			outputDriver.Output();

			return;
        }
	}
}
