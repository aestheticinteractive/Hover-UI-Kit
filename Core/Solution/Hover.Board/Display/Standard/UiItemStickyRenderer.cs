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

	}

}
