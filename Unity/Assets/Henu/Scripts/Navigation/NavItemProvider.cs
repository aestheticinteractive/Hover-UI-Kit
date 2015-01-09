using Henu.Input;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavItemProvider {

		public delegate void ItemChangeHandler(int pDirection);
		public delegate void SelectionHandler(NavItemProvider pNavItemProvider);

		public event ItemChangeHandler OnItemChange;
		public event SelectionHandler OnSelection;

		public InputPointZone Zone { get; private set; }
		public NavItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemProvider(InputPointZone pZone) {
			Zone = pZone;
			OnSelection += (p => {});
			OnItemChange += (d => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Select() {
			OnSelection(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithItem(NavItem pItem, int pDirection) {
			Item = pItem;
			OnItemChange(pDirection);
		}

	}

}
