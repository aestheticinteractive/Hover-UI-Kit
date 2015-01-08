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
			var topC = new NavItemData(NavItemData.ItemType.Parent, "Item C");
			var topD = new NavItemData(NavItemData.ItemType.Checkbox, "Item D");
			var topE = new NavItemData(NavItemData.ItemType.Checkbox, "Item E");
			var topF = new NavItemData(NavItemData.ItemType.Checkbox, "Item F");
			var topG = new NavItemData(NavItemData.ItemType.Checkbox, "Item G");

			var topAItems = new List<NavItemData>();
			topAItems.Add(new NavItemData(NavItemData.ItemType.Radio, "Item A1"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Radio, "Item A2"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Radio, "Item A3"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Radio, "Item A4"));
			topAItems.Add(new NavItemData(NavItemData.ItemType.Radio, "Item A5"));
			topA.SetChildren(topAItems.ToArray());

			var topBItems = new List<NavItemData>();
			topBItems.Add(new NavItemData(NavItemData.ItemType.Checkbox, "Item B1"));
			topBItems.Add(new NavItemData(NavItemData.ItemType.Checkbox, "Item B2"));
			topBItems.Add(new NavItemData(NavItemData.ItemType.Checkbox, "Item B3"));
			topB.SetChildren(topBItems.ToArray());

			var topCItems = new List<NavItemData>();
			topCItems.Add(new NavItemData(NavItemData.ItemType.Parent, "Item C1"));
			topCItems.Add(new NavItemData(NavItemData.ItemType.Parent, "Item C2"));
			topCItems.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C3"));
			topC.SetChildren(topCItems.ToArray());

			var topC1Items = new List<NavItemData>();
			topC1Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C1a"));
			topC1Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C1b"));
			topC1Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C1c"));
			topCItems[0].SetChildren(topC1Items.ToArray());

			var topC2Items = new List<NavItemData>();
			topC2Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C2a"));
			topC2Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C2b"));
			topC2Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C2c"));
			topC2Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C2d"));
			topC2Items.Add(new NavItemData(NavItemData.ItemType.Selection, "Item C2e"));
			topCItems[1].SetChildren(topC2Items.ToArray());

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
