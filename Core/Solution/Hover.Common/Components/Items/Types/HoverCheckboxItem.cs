using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverCheckboxItem : HoverSelectableItem<bool> {

		public new ICheckboxItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverCheckboxItem() {
			Item = new CheckboxItem();
			Init((SelectableItem<bool>)Item);
		}

	}

}
