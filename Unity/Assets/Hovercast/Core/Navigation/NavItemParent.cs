using System;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavItemParent : NavItem<bool> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			ChildLevel = new NavLevel(gameObject);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override NavItem.ItemType Type {
			get {
				return NavItem.ItemType.Parent;
			}
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
