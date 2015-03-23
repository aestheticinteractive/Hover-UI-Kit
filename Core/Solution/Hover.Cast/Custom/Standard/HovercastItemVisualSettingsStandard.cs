using System;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.Display.Default;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.cast.Custom.Standard {

	/*================================================================================================*/
	public class HovercastItemVisualSettingsStandard : HovercastItemVisualSettings {

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
		public Color EdgeColor = new Color(1, 1, 1, 0.5f);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
		public Color SliderTickColor = new Color(1, 1, 1, 0.1f);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IItemVisualSettings GetSettingsInner(IBaseItem pItem) {
			var sett = new ItemVisualSettingsStandard();
			sett.Renderer = GetRendererForItem(pItem);
			sett.TextSize = TextSize;
			sett.TextColor = TextColor;
			sett.TextFont = TextFont;
			sett.ArrowIconColor = ArrowIconColor;
			sett.ToggleIconColor = ToggleIconColor;
			sett.BackgroundColor = BackgroundColor;
			sett.EdgeColor = EdgeColor;
			sett.HighlightColor = HighlightColor;
			sett.SelectionColor = SelectionColor;
			sett.SliderTrackColor = SliderTrackColor;
			sett.SliderFillColor = SliderFillColor;
			sett.SliderTickColor = SliderTickColor;
			return sett;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Type GetRendererForItem(IBaseItem pItem) {
			if ( (pItem as IParentItem) != null ) {
				return typeof(UiParentRenderer);
			}

			if ( (pItem as ISelectorItem) != null ) {
				return typeof(UiSelectRenderer);
			}

			if ( (pItem as IStickyItem) != null ) {
				return typeof(UiStickyRenderer);
			}

			if ( (pItem as ICheckboxItem) != null ) {
				return typeof(UiCheckboxRenderer);
			}

			if ( (pItem as IRadioItem) != null ) {
				return typeof(UiRadioRenderer);
			}

			if ( (pItem as ISliderItem) != null ) {
				return typeof(UiSliderRenderer);
			}

			return typeof(UiSelectRenderer);
		}

	}

}
