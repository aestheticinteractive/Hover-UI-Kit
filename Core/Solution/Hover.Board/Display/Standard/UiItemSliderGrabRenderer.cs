using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderGrabRenderer : UiItemBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Slider");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float sx = vSettings.TextSize*LabelCanvasScale;
			float sy = vSettings.TextSize*1.25f*LabelCanvasScale;
			return new Vector3(sx, sy, 1);
		}

	}

}
