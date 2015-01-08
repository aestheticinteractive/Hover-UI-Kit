using System.Collections.Generic;
using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoData : INavDelegate {

		private readonly NavItemData[] vNavItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoData() {
			var items = new List<NavItemData>();

			var topA = new NavItemData(NavItemData.ItemType.Parent, "Item A");
			var topB = new NavItemData(NavItemData.ItemType.Parent, "Item B");
			var topC = new NavItemData(NavItemData.ItemType.Selection, "Item C");
			var topD = new NavItemData(NavItemData.ItemType.Selection, "Item D");
			var topE = new NavItemData(NavItemData.ItemType.Selection, "Item E");
			var topF = new NavItemData(NavItemData.ItemType.Selection, "Item F");
			var topG = new NavItemData(NavItemData.ItemType.Selection, "Item G");

			var topAItems = new List<NavItemData>();
			topAItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item A1"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item A2"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item A3"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item A4"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item A5"));
			topA.SetChildren(topAItems.ToArray());

			var topBItems = new List<NavItemData>();
			topBItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item B1"));
			topBItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item B2"));
			topBItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item B3"));
			topB.SetChildren(topBItems.ToArray());

			items.Add(topA);
			items.Add(topB);
			items.Add(topC);
			items.Add(topD);
			items.Add(topE);
			items.Add(topF);
			items.Add(topG);

			vNavItems = items.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData[] GetTopLevelItems() {
			return vNavItems;
		}

	}

}
