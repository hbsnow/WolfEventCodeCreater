using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WodiKs.Map.Layer;
using WolfEventCodeCreater.Model.OutputStruct;

namespace WolfEventCodeCreater.Model.WoditerStr
{
	public class MapLayerStr
	{
		public MapDataStr Parent { get; private set; }
		public OutputStructSentence MapLayerID { get; private set; }
		public OutputStructTable LayerChips { get; private set; }

		public MapLayerStr(MapLayer mapLayer, int mapLayerIdNo, MapDataStr mapDataStr)
		{
			Parent = mapDataStr;
			MapLayerID = new OutputStructSentence("マップレイヤー番号", mapLayerIdNo.ToString());
			LayerChips = new OutputStructTable($"レイヤー{MapLayerID.Sentence}の各チップ", SetLayerChipsHeader(), SetLayerChipsData(mapLayer));
		}

		private List<string> SetLayerChipsHeader()
		{
			var layerChipsHeader = new List<string>(){ "y↓ x→" };
			for (int x = 0; x < Parent.Width; x++)
			{
				layerChipsHeader.Add(x.ToString());
			}
			return layerChipsHeader;
		}

		private List<List<string>> SetLayerChipsData(MapLayer mapLayer)
		{
			var layerChipsData = new List<List<string>>();
			long rowNum = mapLayer.LayerChips.Count() / Parent.Width;
			long columnNum = mapLayer.LayerChips.Count() / Parent.Height;

			for(long y = 0; y < rowNum; y++)
			{
				var record = new List<string>() { y.ToString()};
				for(long x = 0; x < columnNum; x++)
				{
					record.Add(x.ToString());
				}
				layerChipsData.Add(record);
			}

			return layerChipsData;
		}
	}
}
