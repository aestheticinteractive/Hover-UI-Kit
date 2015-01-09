using System.Collections.Generic;
using System.Linq;
using Henu.Input;
using Henu.State;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavigationProvider {

		public delegate void LevelChangeHandler(int pDirection);

		public event LevelChangeHandler OnLevelChange;

		private readonly IDictionary<InputPointZone, NavItemProvider> vItemProvMap;
		private readonly Stack<NavItem[]> vHistory;
		private INavDelegate vDelgate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavigationProvider() {
			vItemProvMap = new Dictionary<InputPointZone, NavItemProvider>();
			vHistory = new Stack<NavItem[]>();

			foreach ( InputPointZone zone in MenuHandState.PointZones ) {
				var itemProv = new NavItemProvider(zone);
				itemProv.OnSelection += HandleItemSelection;
				vItemProvMap.Add(zone, itemProv);
			}

			OnLevelChange += (d => { });
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Init(INavDelegate pDelgate) {
			vDelgate = pDelgate;
			vHistory.Clear();
			SetNewItems(vDelgate.GetTopLevelItems(), 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemProvider GetItemProvider(InputPointZone pZone) {
			return vItemProvMap[pZone];
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
		private void HandleItemSelection(NavItemProvider pNavItemProvider) {
			NavItem item = pNavItemProvider.Item;

			if ( item == null ) {
				return;
			}

			switch ( item.Type ) {
				case NavItem.ItemType.Parent:
					PushCurrentItemsToHistory();
					vDelgate.HandleItemSelection(item);
					SetNewItems(item.Children, 1);
					return;

				case NavItem.ItemType.Selection:
				case NavItem.ItemType.Checkbox:
					item.Selected = !item.Selected;
					vDelgate.HandleItemSelection(item);
					return;

				case NavItem.ItemType.Radio:
					SetRadioSelection(item);
					vDelgate.HandleItemSelection(item);
					return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PushCurrentItemsToHistory() {
			NavItem[] items = vItemProvMap.Values.Select(itemProv => itemProv.Item).ToArray();
			vHistory.Push(items);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewItems(NavItem[] pItems, int pDirection) {
			NavItemProvider[] itemProvs = vItemProvMap.Values.ToArray();
			var items = new List<NavItem>();

			for ( int i = 0 ; i < itemProvs.Length ; ++i ) {
				NavItemProvider itemProv = itemProvs[i];
				NavItem item = (pItems == null || i >= pItems.Length ? null : pItems[i]);
				itemProv.UpdateWithItem(item, pDirection);

				if ( item != null ) {
					items.Add(item);
				}
			}

			OnLevelChange(pDirection);
			vDelgate.HandleLevelChange(items.ToArray(), pDirection);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetRadioSelection(NavItem pSelectedItem) {
			foreach ( NavItemProvider itemProv in vItemProvMap.Values ) {
				if ( itemProv.Item == null || itemProv.Item.Type != NavItem.ItemType.Radio ) {
					continue;
				}

				itemProv.Item.Selected = (itemProv.Item == pSelectedItem);
			}
		}

	}

}
