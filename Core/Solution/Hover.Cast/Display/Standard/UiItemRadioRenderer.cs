using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemRadioRenderer : UiItemBaseToggleRenderer {

		private static readonly Texture2D OuterTex = Resources.Load<Texture2D>("Textures/RadioOuter");
		private static readonly Texture2D InnerTex = Resources.Load<Texture2D>("Textures/RadioInner");


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetOuterTexture() {
			return OuterTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Texture2D GetInnerTexture() {
			return InnerTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsToggled() {
			return ((IRadioItem)vItemState.Item).Value;
		}

	}

}
