using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavSelector : HovercastNavItem<NavItemSelector> {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemSelector GetItem() {
			var item = new NavItemSelector();
			FillItem(item);
			return item;
		}
		
	}

}
