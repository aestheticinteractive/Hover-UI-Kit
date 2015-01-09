using Henu.Navigation;
using UnityEngine;

namespace Henu {

	/*================================================================================================*/
	public class HenuNavComponent : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual INavDelegate GetNavDelegate() {
			return new EmptyDelgate();
		}

	}


	/*================================================================================================*/
	public class EmptyDelgate : INavDelegate {

		private readonly NavItemData[] vNavItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public EmptyDelgate() {
			vNavItems = new NavItemData[7];

			for ( int i = 0 ; i < 7 ; i++ ) {
				vNavItems[i] = new NavItemData(NavItemData.ItemType.Checkbox, "Item "+i);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData[] GetTopLevelItems() {
			return vNavItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItemData pItemData) {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItemData[] pItemDataList, int pDirection) {
			//do nothing...
		}

	}

}
