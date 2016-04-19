using System;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class CheckboxItem : SelectableItemBool, ICheckboxItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = !Value;
			base.Select();
		}

	}

}
