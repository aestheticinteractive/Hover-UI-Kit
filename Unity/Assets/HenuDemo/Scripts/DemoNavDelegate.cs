using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void ColorChangeHandler(NavItem pItem);
		public delegate void MotionChangeHandler(NavItem pItem);
		public delegate void LightPosChangeHandler(NavItem pItem);
		public delegate void LightIntenChangeHandler(NavItem pItem);

		public event ColorChangeHandler OnColorChange;
		public event MotionChangeHandler OnMotionChange;
		public event LightPosChangeHandler OnLightPosChange;
		public event LightIntenChangeHandler OnLightIntenChange;

		public DemoNavItems Items { get; private set; }

		private readonly NavItem[] vTopLevelItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();
			vTopLevelItems = new[] { Items.Colors, Items.Motions, Items.LightPos, Items.LightInten };

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

			if ( Items.IsItemWithin(pItem, Items.LightPos) ) {
				OnLightPosChange(pItem);
			}

			if ( Items.IsItemWithin(pItem, Items.LightInten) ) {
				OnLightIntenChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
		}

	}

}
