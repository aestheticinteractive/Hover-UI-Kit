using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiRadioRenderer : UiBaseToggleRenderer {

		private static readonly Texture2D OuterTex = Resources.Load<Texture2D>("RadioOuter");
		private static readonly Texture2D InnerTex = Resources.Load<Texture2D>("RadioInner");


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
