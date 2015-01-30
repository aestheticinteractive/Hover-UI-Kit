using System;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavItemParent : NavItem<bool> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemParent(string pLabel, float pRelativeSize=1) : 
														base(ItemType.Parent, pLabel, pRelativeSize) {
			ChildLevel = new NavLevel();
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
			set {
				throw new Exception("Cannot set NavigateBackUponSelect for 'Parent' NavItems.");
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
