using System;
using Hoverboard.Core.Display.Default;
using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public class HoverboardDefaultCursor : HoverboardCustomCursor {

		public Color ColorNormal = new Color(1, 1, 1, 0.6f);
		public Color ColorHighlighted = new Color(1, 1, 1, 1);
		public float RadiusNormal = 0.12f;
		public float RadiusHighlighted = 0.06f;
		public float ThicknessNormal = 0.1f;
		public float ThicknessHighlighted = 0.4f;

		private CursorSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererInner() {
			return typeof(UiCursorRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override CursorSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new CursorSettings();
				vSettings.ColorNorm = ColorNormal;
				vSettings.ColorHigh = ColorHighlighted;
				vSettings.RadiusNorm = RadiusNormal;
				vSettings.RadiusHigh = RadiusHighlighted;
				vSettings.ThickNorm = ThicknessNormal;
				vSettings.ThickHigh = ThicknessHighlighted;
			}

			return vSettings;
		}

	}

}
