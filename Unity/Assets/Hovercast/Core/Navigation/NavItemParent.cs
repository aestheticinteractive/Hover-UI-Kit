using System;
using System.Collections.Generic;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavItemParent : NavItem<bool> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemParent(Func<NavItem[]> pGetItems) : base(ItemType.Parent) {
			ChildLevel = new NavLevel(pGetItems);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = true;
			base.Select();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool NavigateBackUponSelect {
			get {
				return false;
			}
			internal set {
				if ( value ) {
					throw new Exception("Cannot set NavigateBackUponSelect for 'Parent' NavItems.");
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal override void UpdateValueOnLevelChange(int pDirection) {
			if ( pDirection == -1 ) {
				Value = false;
			}
		}

	}

}
