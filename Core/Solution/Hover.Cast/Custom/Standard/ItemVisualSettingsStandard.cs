using System;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Cast.Custom.Standard {

	/*================================================================================================*/
	public class ItemVisualSettingsStandard : IItemVisualSettings {

		public Type Renderer { get; set; }

		public int TextSize { get; set; }
		public Color TextColor { get; set; }
		public string TextFont { get; set; }
		public Color ArrowIconColor { get; set; }
		public Color ToggleIconColor { get; set; }
		public Color BackgroundColor { get; set; }
		public Color EdgeColor { get; set; }
		public Color HighlightColor { get; set; }
		public Color SelectionColor { get; set; }
		public Color SliderTrackColor { get; set; }
		public Color SliderFillColor { get; set; }
		public Color SliderTickColor { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillWith(IItemVisualSettings pSourceSettings, bool pIncludeRenderer) {
			ItemVisualSettingsStandard sett = (ItemVisualSettingsStandard)pSourceSettings;

			if ( pIncludeRenderer ) {
				Renderer = sett.Renderer;
			}

			TextSize = sett.TextSize;
			TextColor = sett.TextColor;
			TextFont = sett.TextFont;
			ArrowIconColor = sett.ArrowIconColor;
			ToggleIconColor = sett.ToggleIconColor;
			BackgroundColor = sett.BackgroundColor;
			EdgeColor = sett.EdgeColor;
			HighlightColor = sett.HighlightColor;
			SelectionColor = sett.SelectionColor;
			SliderTrackColor = sett.SliderTrackColor;
			SliderFillColor = sett.SliderFillColor;
			SliderTickColor = sett.SliderTickColor;
		}

	}

}
