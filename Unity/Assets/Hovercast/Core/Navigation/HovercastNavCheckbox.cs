using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavCheckbox : HovercastNavItem<NavItemCheckbox> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemCheckbox GetItem() {
			var item = new NavItemCheckbox();
			FillItem(item);
			return item;
		}
		
	}

}
