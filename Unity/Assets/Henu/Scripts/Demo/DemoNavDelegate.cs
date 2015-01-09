using Henu.Navigation;

namespace Henu.Demo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void ColorChangeHandler(NavItemData pItem);
		public event ColorChangeHandler OnColorChange;

		public DemoNavItems Items { get; private set; }

		private readonly NavItemData[] vTopLevelItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();
			vTopLevelItems = new[] { Items.Colors };

			OnColorChange += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData[] GetTopLevelItems() {
			return vTopLevelItems;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItemData pItemData) {
			if ( Items.IsItemWithin(pItemData, Items.Colors) ) {
				OnColorChange(pItemData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItemData[] pItemDataList, int pDirection) {
		}

	}

}
