using Hover.Board.Display.Standard;
using Hover.Common.Custom;
using Hover.Common.Items;
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

			sett.Renderer = typeof(UiItemSelectRenderer); //TODO: FEATURE: handle other item types
			return sett;
		}

	}

}
