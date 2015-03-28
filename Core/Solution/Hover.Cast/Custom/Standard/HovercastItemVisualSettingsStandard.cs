using System;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.Display.Standard;
using Hover.Common.Custom;
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
		public Color EdgeColor = new Color(1, 1, 1, 1);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
		public Color SliderTickColor = new Color(1, 1, 1, 0.2f);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IItemVisualSettings GetSettingsInner(IBaseItem pItem,
																	IItemVisualSettings pDefault=null) {
			var sett = new ItemVisualSettingsStandard();

			if ( pDefault != null ) {
				sett.FillWith(pDefault, false);
			}
			else {
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
			}

			sett.Renderer = GetRendererForItem(pItem);
			return sett;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Type GetRendererForItem(IBaseItem pItem) {
			if ( (pItem as IParentItem) != null ) {
				return typeof(UiItemParentRenderer);
			}

			if ( (pItem as ISelectorItem) != null ) {
				return typeof(UiItemSelectRenderer);
			}

			if ( (pItem as IStickyItem) != null ) {
				return typeof(UiItemStickyRenderer);
			}

			if ( (pItem as ICheckboxItem) != null ) {
				return typeof(UiItemCheckboxRenderer);
			}

			if ( (pItem as IRadioItem) != null ) {
				return typeof(UiItemRadioRenderer);
			}

			if ( (pItem as ISliderItem) != null ) {
				return typeof(UiItemSliderRenderer);
			}

			return typeof(UiItemSelectRenderer);
		}

	}

}
