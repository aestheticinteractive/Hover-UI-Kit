using UnityEngine;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiSliderGrabRenderer : UiBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Slider");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float sx = vSettings.TextSize*ArcCanvasScale;
			float sy = vSettings.TextSize*1.25f*ArcCanvasScale;
			return new Vector3(sx, sy, 1);
		}

	}

}
