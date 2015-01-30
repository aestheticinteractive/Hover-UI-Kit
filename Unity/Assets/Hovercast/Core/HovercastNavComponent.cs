using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public class HovercastNavComponent : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual INavDelegate GetNavDelegate() {
			return new EmptyDelgate();
		}

	}


	/*================================================================================================*/
	public class EmptyDelgate : INavDelegate {

		private readonly NavLevel vNavLevel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public EmptyDelgate() {
			vNavLevel = new NavLevel();
			vNavLevel.Items = new NavItem[7];

			for ( int i = 0 ; i < 7 ; i++ ) {
				vNavLevel.Items[i] = new NavItemCheckbox("Item "+i);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavLevel GetTopLevel() {
			return vNavLevel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetTopLevelTitle() {
			return "Main Menu";
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelection(NavLevel pLevel, NavItem pItem) {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleLevelChange(NavLevel pNewLevel, int pDirection) {
			//do nothing...
		}

	}

}
