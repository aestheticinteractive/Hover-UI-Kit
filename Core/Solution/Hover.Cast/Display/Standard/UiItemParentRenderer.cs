using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemParentRenderer : UiItemBaseIconRenderer {

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Parent");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetIconTexture() {
			return IconTex;
		}

	}

}
