using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavSticky : HovercastNavItem<NavItemSticky> {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemSticky GetItem() {
			var item = new NavItemSticky();
			FillItem(item);
			return item;
		}
		
	}

}
