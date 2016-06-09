using System;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class StickyItem : SelectableItem, IStickyItem {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

	}

}
