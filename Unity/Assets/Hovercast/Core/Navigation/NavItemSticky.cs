namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavItemSticky : NavItem {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override NavItem.ItemType Type {
			get {
				return NavItem.ItemType.Sticky;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

	}

}
