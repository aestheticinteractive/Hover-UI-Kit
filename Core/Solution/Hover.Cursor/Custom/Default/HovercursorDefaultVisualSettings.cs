using Hover.Cursor.Display.Default;
using UnityEngine;

namespace Hover.Cursor.Custom.Default {

	/*================================================================================================*/
	public class HovercursorDefaultVisualSettings : HovercursorVisualSettings {

		public Color ColorNormal = new Color(1, 1, 1, 0.6f);
		public Color ColorHighlighted = new Color(1, 1, 1, 1);
		public float RadiusNormal = 0.12f;
		public float RadiusHighlighted = 0.06f;
		public float ThicknessNormal = 0.1f;
		public float ThicknessHighlighted = 0.4f;
		public float CursorForwardDistance = 0;

		private CursorSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override CursorSettings GetSettingsInner() {
			if ( vSettings == null ) {
				vSettings = new CursorSettings();
				vSettings.Renderer = typeof(UiCursorRenderer);
				vSettings.ColorNorm = ColorNormal;
				vSettings.ColorHigh = ColorHighlighted;
				vSettings.RadiusNorm = RadiusNormal;
				vSettings.RadiusHigh = RadiusHighlighted;
				vSettings.ThickNorm = ThicknessNormal;
				vSettings.ThickHigh = ThicknessHighlighted;
				vSettings.CursorForwardDistance = CursorForwardDistance;
			}

			return vSettings;
		}

	}

}
