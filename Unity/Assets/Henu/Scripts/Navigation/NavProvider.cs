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
		private readonly Stack<NavItemData[]> vHistory;
		private INavDelegate vDelgate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavigationProvider() {
			vItemProvMap = new Dictionary<InputPointZone, NavItemProvider>();
			vHistory = new Stack<NavItemData[]>();

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
			NavItemData itemData = pNavItemProvider.Data;

			if ( itemData == null ) {
				return;
			}

			switch ( itemData.Type ) {
				case NavItemData.ItemType.Parent:
					PushCurrentItemsToHistory();
					vDelgate.HandleItemSelection(itemData);
					SetNewItems(itemData.Children, 1);
					return;

				case NavItemData.ItemType.Selection:
				case NavItemData.ItemType.Checkbox:
					itemData.Selected = !itemData.Selected;
					vDelgate.HandleItemSelection(itemData);
					return;

				case NavItemData.ItemType.Radio:
					SetRadioSelection(itemData);
					vDelgate.HandleItemSelection(itemData);
					return;
			}

		}

		/*--------------------------------------------------------------------------------------------*/
		private void PushCurrentItemsToHistory() {
			NavItemData[] items = vItemProvMap.Values.Select(itemProv => itemProv.Data).ToArray();
			vHistory.Push(items);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewItems(NavItemData[] pItems, int pDirection) {
			NavItemProvider[] itemProvs = vItemProvMap.Values.ToArray();
			var itemDataList = new List<NavItemData>();

			for ( int i = 0 ; i < itemProvs.Length ; ++i ) {
				NavItemProvider itemProv = itemProvs[i];
				NavItemData itemData = (pItems == null || i >= pItems.Length ? null : pItems[i]);
				itemProv.UpdateWithData(itemData, pDirection);

				if ( itemData != null ) {
					itemDataList.Add(itemData);
				}
			}

			OnLevelChange(pDirection);
			vDelgate.HandleLevelChange(itemDataList.ToArray(), pDirection);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetRadioSelection(NavItemData pSelectedItemData) {
			foreach ( NavItemProvider itemProv in vItemProvMap.Values ) {
				if ( itemProv.Data.Type != NavItemData.ItemType.Radio ) {
					continue;
				}

				itemProv.Data.Selected = (itemProv.Data == pSelectedItemData);
			}
		}

	}

}
