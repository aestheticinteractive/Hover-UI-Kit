using Hover.Board.Display.Standard;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Custom.Standard {

	/*================================================================================================*/
	public class HoverboardItemVisualSettingsStandard : HoverboardItemVisualSettings {

		public static Color Green = new Color(0.1f, 0.9f, 0.2f);

		public int TextSize = 36;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
		public Color EdgeColor = new Color(1, 1, 1, 1);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = Green;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IItemVisualSettings GetSettingsInner(IBaseItem pItem) {
			var sett = new ItemVisualSettingsStandard();
			sett.Renderer = typeof(UiSelectRenderer); //TODO: handle other item types
			sett.TextSize = TextSize;
			sett.TextColor = TextColor;
			sett.TextFont = TextFont;
			sett.ArrowIconColor = ArrowIconColor;
			sett.ToggleIconColor = ToggleIconColor;
			sett.BackgroundColor = BackgroundColor;
			sett.EdgeColor = EdgeColor;
			sett.HighlightColor = HighlightColor;
			sett.SelectionColor = SelectionColor;
			return sett;
		}

	}

}
