using Hover.Cursor.Display.Standard;
using UnityEngine;

namespace Hover.Cursor.Custom.Standard {

	/*================================================================================================*/
	public class HovercursorVisualSettingsStandard : HovercursorVisualSettings {

		public Color ColorNormal = new Color(1, 1, 1, 0.6f);
		public Color ColorHighlighted = new Color(1, 1, 1, 1);
		public float RadiusNormal = 0.12f;
		public float RadiusHighlighted = 0.06f;
		public float ThicknessNormal = 0.1f;
		public float ThicknessHighlighted = 0.4f;
		public float CursorForwardDistance = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override ICursorSettings GetSettingsInner() {
			var sett = new CursorSettingsStandard();
			sett.Renderer = typeof(UiCursorRenderer);
			sett.ColorNorm = ColorNormal;
			sett.ColorHigh = ColorHighlighted;
			sett.RadiusNorm = RadiusNormal;
			sett.RadiusHigh = RadiusHighlighted;
			sett.ThickNorm = ThicknessNormal;
			sett.ThickHigh = ThicknessHighlighted;
			sett.CursorForwardDistance = CursorForwardDistance;
			return sett;
		}

	}

}
