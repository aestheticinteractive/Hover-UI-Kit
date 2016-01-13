using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverParentItem : HoverSelectableItem {

		public new IParentItem Item { get; private set; }

		//TODO: hide the NavigateBackUponSelect property
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverParentItem() {
			Item = new ParentItem(() => GetChildItems(gameObject));
			Init((SelectableItem)Item);
		}

	}

}
