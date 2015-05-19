using Hover.Common.Display;
using Hover.Common.Items.Types;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemRadioRenderer : UiItemBaseToggleRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetOuterIconOffset() {
			return Materials.IconOffset.RadioOuter;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetInnerIconOffset() {
			return Materials.IconOffset.RadioInner;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsToggled() {
			return ((IRadioItem)vItemState.Item).Value;
		}

	}

}
