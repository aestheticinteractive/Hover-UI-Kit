using System;

namespace Hover.Common.Items.Types {

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
