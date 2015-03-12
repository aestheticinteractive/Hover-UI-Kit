using System;
using Hoverboard.Core.Display.Default;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public class HoverboardDefaultButton : HoverboardCustomButton {

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

		private ButtonSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererForNavItemTypeInner(NavItem.ItemType pNavItemType) {
			return typeof(UiSelectRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override ButtonSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new ButtonSettings();
				vSettings.TextSize = TextSize;
				vSettings.TextColor = TextColor;
				vSettings.TextFont = TextFont;
				vSettings.ArrowIconColor = ArrowIconColor;
				vSettings.ToggleIconColor = ToggleIconColor;
				vSettings.BackgroundColor = BackgroundColor;
				vSettings.EdgeColor = EdgeColor;
				vSettings.HighlightColor = HighlightColor;
				vSettings.SelectionColor = SelectionColor;
			}

			return vSettings;
		}

	}

}
