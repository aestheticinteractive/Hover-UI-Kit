using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {
	
	/*================================================================================================*/
	public class HovercastNavRadio : HovercastNavItem<NavItemRadio, bool> {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemRadio GetItem() {
			var item = new NavItemRadio();
			FillItem(item);
			return item;
		}
		
	}

}
