using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiCheckboxRenderer : UiBaseToggleRenderer {

		private static readonly Texture2D OuterTex = Resources.Load<Texture2D>("CheckboxOuter");
		private static readonly Texture2D InnerTex = Resources.Load<Texture2D>("CheckboxInner");


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
			return ((ICheckboxItem)vItemState.Item).Value;
		}

	}

}
