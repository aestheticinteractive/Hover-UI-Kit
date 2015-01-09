using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void ColorChangeHandler(NavItem pItem);
		public event ColorChangeHandler OnColorChange;

		public DemoNavItems Items { get; private set; }

		private readonly NavItem[] vTopLevelItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();
			vTopLevelItems = new[] { Items.Colors };

			OnColorChange += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] GetTopLevelItems() {
			return vTopLevelItems;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItem pItem) {
			if ( Items.IsItemWithin(pItem, Items.Colors) ) {
				OnColorChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
		}

	}

}
