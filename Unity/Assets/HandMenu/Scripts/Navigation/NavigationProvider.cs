using System.Collections.Generic;
using System.Linq;
using HandMenu.State;

namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class NavigationProvider {

		private readonly IDictionary<PointData.PointZone, ItemProvider> vItemProvMap;
		private readonly Stack<ItemData[]> vHistory;
		private INavigationDelegate vDelgate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavigationProvider() {
			vItemProvMap = new Dictionary<PointData.PointZone, ItemProvider>();
			vHistory = new Stack<ItemData[]>();

			foreach ( PointData.PointZone zone in MenuHandState.PointZones ) {
				var itemProv = new ItemProvider(zone);
				itemProv.OnSelection += HandleItemSelection;
				vItemProvMap.Add(zone, itemProv);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Init(INavigationDelegate pDelgate) {
			vDelgate = pDelgate;
			vHistory.Clear();
			SetNewItems(vDelgate.GetTopLevelItems());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemProvider GetItemProvider(PointData.PointZone pZone) {
			return vItemProvMap[pZone];
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Back() {
			if ( vHistory.Count == 0 ) {
				return;
			}

			SetNewItems(vHistory.Pop());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelection(ItemProvider pItemProvider) {
			ItemData itemData = pItemProvider.Data;

			if ( itemData == null ) {
				return;
			}

			if ( itemData.Type == ItemData.ItemType.Parent ) {
				PushCurrentItemsToHistory();
				SetNewItems(itemData.Children);
				return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PushCurrentItemsToHistory() {
			ItemData[] items = vItemProvMap.Values.Select(itemProv => itemProv.Data).ToArray();
			vHistory.Push(items);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewItems(ItemData[] pItems) {
			ItemProvider[] itemProvs = vItemProvMap.Values.ToArray();

			for ( int i = 0 ; i < itemProvs.Length ; ++i ) {
				ItemProvider itemProv = itemProvs[i];
				ItemData itemData = (pItems == null || i >= pItems.Length ? null : pItems[i]);
				itemProv.UpdateWithData(itemData);
			}
		}

	}

}
