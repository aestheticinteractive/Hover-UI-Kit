using System.Collections.Generic;
using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoData : INavigationDelegate {

		private readonly ItemData[] vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoData() {
			var items = new List<ItemData>();

			var topA = new ItemData(ItemData.ItemType.Parent, "Item A");
			var topB = new ItemData(ItemData.ItemType.Parent, "Item B");
			var topC = new ItemData(ItemData.ItemType.Selection, "Item C");
			var topD = new ItemData(ItemData.ItemType.Selection, "Item D");
			var topE = new ItemData(ItemData.ItemType.Selection, "Item E");
			var topF = new ItemData(ItemData.ItemType.Selection, "Item F");
			var topG = new ItemData(ItemData.ItemType.Selection, "Item G");

			var topAItems = new List<ItemData>();
			topAItems.Add(new ItemData(ItemData.ItemType.Selection, "Item A1"));
			topAItems.Add(new ItemData(ItemData.ItemType.Selection, "Item A2"));
			topAItems.Add(new ItemData(ItemData.ItemType.Selection, "Item A3"));
			topAItems.Add(new ItemData(ItemData.ItemType.Selection, "Item A4"));
			topAItems.Add(new ItemData(ItemData.ItemType.Selection, "Item A5"));
			topA.SetChildren(topAItems.ToArray());

			var topBItems = new List<ItemData>();
			topBItems.Add(new ItemData(ItemData.ItemType.Selection, "Item B1"));
			topBItems.Add(new ItemData(ItemData.ItemType.Selection, "Item B2"));
			topBItems.Add(new ItemData(ItemData.ItemType.Selection, "Item B3"));
			topB.SetChildren(topBItems.ToArray());

			items.Add(topA);
			items.Add(topB);
			items.Add(topC);
			items.Add(topD);
			items.Add(topE);
			items.Add(topF);
			items.Add(topG);

			vItems = items.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemData[] GetTopLevelItems() {
			return vItems;
		}

	}

}
