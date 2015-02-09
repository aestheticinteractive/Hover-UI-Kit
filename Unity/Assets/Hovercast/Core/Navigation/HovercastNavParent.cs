using System;
using UnityEngine;
using System.Collections.Generic;

namespace Hovercast.Core.Navigation {
	
	/*================================================================================================*/
	public class HovercastNavParent : HovercastNavItem<NavItemParent> {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemParent GetItem() {
			var item = new NavItemParent(GetChildItems);
			FillItem(item);
			return item;
		}

		/*--------------------------------------------------------------------------------------------*/
		private NavItem[] GetChildItems() {
			return HovercastNavProvider.GetChildItems(gameObject);
		}
		
	}

}
