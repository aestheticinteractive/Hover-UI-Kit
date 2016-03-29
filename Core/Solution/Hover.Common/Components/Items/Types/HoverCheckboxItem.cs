using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverCheckboxItem : HoverSelectableItemBool, ICheckboxItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = !Value;
			base.Select();
		}

	}

}
