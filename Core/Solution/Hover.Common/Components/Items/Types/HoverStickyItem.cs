using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverStickyItem : HoverSelectableItem, IStickyItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

	}

}
