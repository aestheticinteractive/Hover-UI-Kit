using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverStickyItem : HoverSelectableItem {

		public new IStickyItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverStickyItem() {
			Item = new StickyItem();
			Init((SelectableItem)Item);
		}

	}

}
