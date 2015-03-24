using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.Display.Standard;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.cast.Custom.Standard {

	/*================================================================================================*/
	public class HovercastPalmVisualSettingsStandard : HovercastPalmVisualSettings {

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IPalmVisualSettings GetSettingsInner(IBaseItem pItem) {
			var sett = new PalmVisualSettingsStandard();
			sett.Renderer = typeof(UiPalmRenderer);
			sett.TextSize = TextSize;
			sett.TextColor = TextColor;
			sett.TextFont = TextFont;
			sett.BackgroundColor = BackgroundColor;
			return sett;
		}

	}

}
