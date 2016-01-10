using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSelectorItem : HoverSelectableItem {

		public new ISelectorItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectorItem() {
			Item = new SelectorItem();
			Init((SelectableItem)Item);
		}

	}

}
