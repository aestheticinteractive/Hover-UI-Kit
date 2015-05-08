using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemStickyRenderer : UiItemBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Sticky");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float s = vSettings.TextSize*LabelCanvasScale;
			return new Vector3(s, s, 1);
		}

	}

}
