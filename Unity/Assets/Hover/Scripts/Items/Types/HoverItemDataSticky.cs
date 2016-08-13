using System;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSticky : HoverItemDataSelectable, IItemDataSticky {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

	}

}
