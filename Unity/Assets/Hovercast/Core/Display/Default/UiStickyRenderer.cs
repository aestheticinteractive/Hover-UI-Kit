using UnityEngine;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiStickyRenderer : UiBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Sticky");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float s = vSettings.TextSize*ArcCanvasScale;
			return new Vector3(s, s, 1);
		}

	}

}
