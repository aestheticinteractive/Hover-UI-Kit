using System;
using Hovercast.Core.Display.Default;
using UnityEngine;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public class HovercastDefaultCursor : HovercastCustomCursor {

		public Color ColorNormal = new Color(1, 1, 1, 0.6f);
		public Color ColorHighlighted = new Color(1, 1, 1, 1);
		public float RadiusNormal = 0.12f;
		public float RadiusHighlighted = 0.06f;
		public float ThicknessNormal = 0.1f;
		public float ThicknessHighlighted = 0.4f;

		private CursorSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vSettings = new CursorSettings();
			vSettings.ColorNorm = ColorNormal;
			vSettings.ColorHigh = ColorHighlighted;
			vSettings.RadiusNorm = RadiusNormal;
			vSettings.RadiusHigh = RadiusHighlighted;
			vSettings.ThickNorm = ThicknessNormal;
			vSettings.ThickHigh = ThicknessHighlighted;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererInner() {
			return typeof(UiCursorRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override CursorSettings GetSettings() {
			return vSettings;
		}

	}

}
