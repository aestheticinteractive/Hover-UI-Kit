using System;
using Hover.Cast.Display.Default;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastDefaultSegment : HovercastCustomSegment {

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

		private SegmentSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererForItemInner(IBaseItem pItem) {
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

		/*--------------------------------------------------------------------------------------------*/
		public override SegmentSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new SegmentSettings();
				vSettings.TextSize = TextSize;
				vSettings.TextColor = TextColor;
				vSettings.TextFont = TextFont;
				vSettings.ArrowIconColor = ArrowIconColor;
				vSettings.ToggleIconColor = ToggleIconColor;
				vSettings.BackgroundColor = BackgroundColor;
				vSettings.EdgeColor = EdgeColor;
				vSettings.HighlightColor = HighlightColor;
				vSettings.SelectionColor = SelectionColor;
				vSettings.SliderTrackColor = SliderTrackColor;
				vSettings.SliderFillColor = SliderFillColor;
				vSettings.SliderTickColor = SliderTickColor;
			}

			return vSettings;
		}

	}

}
