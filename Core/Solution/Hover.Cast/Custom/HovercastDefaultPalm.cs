using System;
using Hover.Cast.Display.Default;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastDefaultPalm : HovercastCustomPalm {

		public bool InheritSettings = true;

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);

		private ItemVisualSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererInner() {
			return typeof(UiPalmRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override ItemVisualSettings GetSettings() {
			if ( InheritSettings ) {
				return null;
			}

			if ( vSettings == null ) {
				vSettings = new ItemVisualSettings();
				vSettings.TextSize = TextSize;
				vSettings.TextColor = TextColor;
				vSettings.TextFont = TextFont;
				vSettings.BackgroundColor = BackgroundColor;
			}

			return vSettings;
		}

	}

}
