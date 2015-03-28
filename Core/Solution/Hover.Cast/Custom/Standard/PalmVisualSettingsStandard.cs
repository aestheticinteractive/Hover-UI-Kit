using System;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Cast.Custom.Standard {

	/*================================================================================================*/
	public class PalmVisualSettingsStandard : IPalmVisualSettings {

		public Type Renderer { get; set; }

		public int TextSize { get; set; }
		public Color TextColor { get; set; }
		public string TextFont { get; set; }
		public Color BackgroundColor { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillWith(IItemVisualSettings pSourceSettings, bool pIncludeRenderer) {
			PalmVisualSettingsStandard sett = (PalmVisualSettingsStandard)pSourceSettings;

			if ( pIncludeRenderer ) {
				Renderer = sett.Renderer;
			}

			TextSize = sett.TextSize;
			TextColor = sett.TextColor;
			TextFont = sett.TextFont;
			BackgroundColor = sett.BackgroundColor;
		}

	}

}
