using System.Collections.Generic;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavigationProvider {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		public NavItem ActiveParentItem { get; private set; }

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
		public bool IsAtTopLevelMenu() {
			return (vHistory.Count == 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return vDelgate.GetTopLevelTitle();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Back() {
			if ( vHistory.Count == 0 ) {
				return;
			}

			NavItem[] items = vHistory.Pop();

			foreach ( NavItem item in items ) {
				if ( item.Type == NavItem.ItemType.Parent ) {
					item.Selected = false;
				}
			}

			if ( vHistory.Count > 0 ) {
				NavItem[] parentItems = vHistory.Peek();

				foreach ( NavItem item in parentItems ) {
					if ( item.Type == NavItem.ItemType.Parent && item.Selected ) {
						ActiveParentItem = item;
					}
				}
			}
			else {
				ActiveParentItem = null;
			}

			SetNewItems(items, -1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelection(NavItem pItem) {
			if ( pItem.Type == NavItem.ItemType.Parent ) {
				pItem.Selected = true;
				ActiveParentItem = pItem;

				vDelgate.HandleItemSelection(pItem);
				PushCurrentItemsToHistory();
				SetNewItems(pItem.Children, 1);
				return;
			}

			switch ( pItem.Type ) {
				case NavItem.ItemType.Selection:
					pItem.Selected = true;
					break;

				case NavItem.ItemType.Checkbox:
					pItem.Selected = !pItem.Selected;
					break;

				case NavItem.ItemType.Radio:
					SetRadioSelection(pItem);
					break;
			}

			vDelgate.HandleItemSelection(pItem);

			if ( pItem.NavigateBackUponSelect ) {
				Back();
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
