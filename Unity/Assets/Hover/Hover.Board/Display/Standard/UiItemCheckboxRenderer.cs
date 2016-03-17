using Hover.Common.Display;
using Hover.Common.Items.Types;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemCheckboxRenderer : UiItemBaseToggleRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetOuterIconOffset() {
			return Materials.IconOffset.CheckOuter;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetInnerIconOffset() {
			return Materials.IconOffset.CheckInner;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsToggled() {
			return ((ICheckboxItem)vItemState.Item).Value;
		}

	}

}
