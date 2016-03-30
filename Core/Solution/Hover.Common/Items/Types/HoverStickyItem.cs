namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class HoverStickyItem : HoverSelectableItem, IStickyItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

	}

}
