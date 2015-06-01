using System;
using Hover.Board.Display.Standard;
using Hover.Common.Custom;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Board.Custom.Standard {

	/*================================================================================================*/
	public class HoverboardItemVisualSettingsStandard : HoverboardItemVisualSettings {

		public static Color Green = new Color(0.1f, 0.9f, 0.2f);

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
		public Color EdgeColor = new Color(1, 1, 1, 1);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = Green;
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
		public Color SliderTickColor = new Color(1, 1, 1, 0.2f);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IItemVisualSettings GetSettingsInner(IBaseItem pItem,
												IItemVisualSettings pDefault, bool pFillWithDefault) {
			var sett = new ItemVisualSettingsStandard();

			if ( pFillWithDefault ) {
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

			if ( (pItem as ITextItem) != null ) {
				return typeof(UiItemTextRenderer);
			}

			return typeof(UiItemSelectRenderer);
		}

	}

}
