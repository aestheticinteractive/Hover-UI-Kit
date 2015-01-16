using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavDelegate : INavDelegate {

		public delegate void MotionChangeHandler(NavItem pItem);
		public delegate void CameraChangeHandler(NavItem pItem);

		public event MotionChangeHandler OnMotionChange;
		public event CameraChangeHandler OnCameraChange;

		public DemoNavItems Items { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavDelegate() {
			Items = new DemoNavItems();

			OnMotionChange += (i => {});
			OnCameraChange += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] GetTopLevelItems() {
			return Items.TopLevelItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return "Henu Demo";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItem pItem) {
			if ( DemoNavItems.IsItemWithin(pItem, Items.Motion, NavItem.ItemType.Checkbox) ) {
				OnMotionChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.Camera, NavItem.ItemType.Radio) ) {
				OnCameraChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
		}

	}

}
