using System.Collections.Generic;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavigationProvider {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		public NavItemParent ActiveParentItem { get; private set; }

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
				item.OnSelected += HandleItemSelected;
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
				item.UpdateValueOnLevelChange(-1);
			}

			FindNewActiveParentItem();
			SetNewItems(items, -1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(NavItem pItem) {
			if ( pItem.Type == NavItem.ItemType.Parent ) {
				ActiveParentItem = (NavItemParent)pItem;

				vDelgate.HandleItemSelection(pItem);
				PushCurrentItemsToHistory();
				SetNewItems(pItem.Children, 1);
				return;
			}

			////

			if ( pItem.Type == NavItem.ItemType.Radio ) {
				DeselectRadioSiblings(pItem);
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
		private void FindNewActiveParentItem() {
			if ( vHistory.Count == 0 ) {
				ActiveParentItem = null;
				return;
			}

			NavItem[] parItems = vHistory.Peek();

			foreach ( NavItem item in parItems ) {
				NavItemParent parItem = (item as NavItemParent);

				if ( parItem == null || !parItem.Value ) {
					continue;
				}

				ActiveParentItem = parItem;
				break;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewItems(NavItem[] pItems, int pDirection) {
			if ( vItems != null ) {
				foreach ( NavItem item in vItems ) {
					item.DeselectStickySelections();
				}
			}

			vItems = pItems;
			OnLevelChange(pDirection);
			vDelgate.HandleLevelChange(vItems, pDirection);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRadioSiblings(NavItem pSelectedItem) {
			foreach ( NavItem item in vItems ) {
				if ( item == pSelectedItem ) {
					continue;
				}

				NavItemRadio radItem = (item as NavItemRadio);

				if ( radItem == null ) {
					continue;
				}

				radItem.Value = false;
			}
		}

	}

}
