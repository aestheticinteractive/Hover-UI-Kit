using System;
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
		public static ItemVisualSettingsStandard Fill(
									ItemVisualSettingsStandard pSrc, ItemVisualSettingsStandard pDest) {
			pDest.Renderer = pSrc.Renderer;
			pDest.TextSize = pSrc.TextSize;
			pDest.TextColor = pSrc.TextColor;
			pDest.TextFont = pSrc.TextFont;
			pDest.ArrowIconColor = pSrc.ArrowIconColor;
			pDest.ToggleIconColor = pSrc.ToggleIconColor;
			pDest.BackgroundColor = pSrc.BackgroundColor;
			pDest.EdgeColor = pSrc.EdgeColor;
			pDest.HighlightColor = pSrc.HighlightColor;
			pDest.SelectionColor = pSrc.SelectionColor;
			pDest.SliderTrackColor = pSrc.SliderTrackColor;
			pDest.SliderFillColor = pSrc.SliderFillColor;
			pDest.SliderTickColor = pSrc.SliderTickColor;
			return pDest;
		}

	}

}
