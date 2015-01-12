using System.Collections.Generic;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavigationProvider {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		private readonly Stack<NavItem[]> vHistory;
		private NavItem[] vItems;
		private INavDelegate vDelgate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavigationProvider() {
			vHistory = new Stack<NavItem[]>();
			OnLevelChange += (d => {});
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Init(INavDelegate pDelgate) {
			vDelgate = pDelgate;
			vHistory.Clear();

			NavItem[] items = vDelgate.GetTopLevelItems();
			PrepareItems(items);
			SetNewItems(items, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PrepareItems(NavItem[] pItems) {
			if ( pItems == null ) {
				return;
			}

			foreach ( NavItem item in pItems ) {
				item.OnSelection += HandleItemSelection;
				PrepareItems(item.Children);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] GetItems() {
			return vItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Back() {
			if ( vHistory.Count == 0 ) {
				return;
			}

			SetNewItems(vHistory.Pop(), -1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelection(NavItem pItem) {
			if ( pItem == null ) {
				return;
			}

			switch ( pItem.Type ) {
				case NavItem.ItemType.Parent:
					vDelgate.HandleItemSelection(pItem);
					PushCurrentItemsToHistory();
					SetNewItems(pItem.Children, 1);
					return;

				case NavItem.ItemType.Selection:
				case NavItem.ItemType.Checkbox:
					pItem.Selected = !pItem.Selected;
					vDelgate.HandleItemSelection(pItem);
					return;

				case NavItem.ItemType.Radio:
					SetRadioSelection(pItem);
					vDelgate.HandleItemSelection(pItem);
					return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PushCurrentItemsToHistory() {
			vHistory.Push(vItems);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewItems(NavItem[] pItems, int pDirection) {
			vItems = pItems;
			OnLevelChange(pDirection);
			vDelgate.HandleLevelChange(vItems, pDirection);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetRadioSelection(NavItem pSelectedItem) {
			foreach ( NavItem item in vItems ) {
				if ( item.Type != NavItem.ItemType.Radio ) {
					continue;
				}

				item.Selected = (item == pSelectedItem);
			}
		}

	}

}
