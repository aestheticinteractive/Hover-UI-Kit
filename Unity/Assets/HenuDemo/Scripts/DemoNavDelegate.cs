using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void ColorChangeHandler(NavItem pItem);
		public delegate void MotionChangeHandler(NavItem pItem);
		public delegate void LightPosChangeHandler(NavItem pItem);
		public delegate void LightIntenChangeHandler(NavItem pItem);
		public delegate void CameraPosChangeHandler(NavItem pItem);

		public event ColorChangeHandler OnColorChange;
		public event MotionChangeHandler OnMotionChange;
		public event LightPosChangeHandler OnLightPosChange;
		public event LightIntenChangeHandler OnLightIntenChange;
		public event CameraPosChangeHandler OnCameraPosChange;

		public DemoNavItems Items { get; private set; }

		private readonly NavItem[] vTopLevelItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();
			vTopLevelItems = new[] { Items.Colors, Items.Motions, Items.LightPos, Items.Custom,
				Items.LightInten, Items.CameraPos };

			OnColorChange += (i => {});
			OnMotionChange += (i => {});
			OnLightPosChange += (i => { });
			OnLightIntenChange += (i => { });
			OnCameraPosChange += (i => { });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] GetTopLevelItems() {
			return vTopLevelItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return "Henu Demo";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItem pItem) {
			if ( DemoNavItems.IsItemWithin(pItem, Items.Colors) ) {
				OnColorChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.Motions) ) {
				OnMotionChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.LightPos) ) {
				OnLightPosChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.LightInten) ) {
				OnLightIntenChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.CameraPos) ) {
				OnCameraPosChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
		}

	}

}
