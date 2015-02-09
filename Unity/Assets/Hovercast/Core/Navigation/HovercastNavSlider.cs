using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavSlider : HovercastNavItem<NavItemSlider, float> {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override NavItemSlider GetItem() {
			var item = new NavItemSlider();
			FillItem(item);
			return item;
		}
		
	}

}
