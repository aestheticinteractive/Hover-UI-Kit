using Hovercast.Core.Navigation;

namespace Hovercast.Demo {

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
		public NavLevel GetTopLevel() {
			return Items.TopLevel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return "Hovercast VR";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavLevel pLevel, NavItem pItem) {
			if ( DemoNavItems.IsItemWithin(pItem, Items.Motion, NavItem.ItemType.Checkbox) ) {
				OnMotionChange(pItem);
			}

			if ( DemoNavItems.IsItemWithin(pItem, Items.Camera, NavItem.ItemType.Radio) ) {
				OnCameraChange(pItem);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavLevel pNewLevel, int pDirection) {
		}

	}

}
