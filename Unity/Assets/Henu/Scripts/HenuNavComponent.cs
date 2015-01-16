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

		private readonly NavItem[] vNavItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public EmptyDelgate() {
			vNavItems = new NavItem[7];

			for ( int i = 0 ; i < 7 ; i++ ) {
				vNavItems[i] = new NavItemCheckbox("Item "+i);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] GetTopLevelItems() {
			return vNavItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return "Main Menu";
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavItem pItem) {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavItem[] pItemList, int pDirection) {
			//do nothing...
		}

	}

}
