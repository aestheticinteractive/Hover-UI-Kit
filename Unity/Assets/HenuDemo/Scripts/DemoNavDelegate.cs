using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void ColorChangeHandler(NavItem pItem);
		public delegate void MotionChangeHandler(NavItem pItem);

		public event ColorChangeHandler OnColorChange;
		public event MotionChangeHandler OnMotionChange;

		public DemoNavItems Items { get; private set; }

		private readonly NavItem[] vTopLevelItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();
			vTopLevelItems = new[] { Items.Colors, Items.Motions };

			OnColorChange += (i => {});
			OnMotionChange += (i => {});
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
			
			if ( Items.IsItemWithin(pItem, Items.Motions) ) {
				OnMotionChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
		}

	}

}
