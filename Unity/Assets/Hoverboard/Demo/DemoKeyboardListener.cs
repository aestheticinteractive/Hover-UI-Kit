using System.Linq;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoKeyboardListener : MonoBehaviour {

		private DemoEnvironment vEnviro;
		private DemoTextField vTextField;
		private GameObject vKeyboardObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vEnviro = GameObject.Find("DemoEnvironment").GetComponent<DemoEnvironment>();
			vTextField = GameObject.Find("DemoTextField").GetComponent<DemoTextField>();
			vKeyboardObj = GameObject.Find("SplitKeyboard");

			NavPanel[] navPanels = vKeyboardObj
				.GetComponentsInChildren<HoverboardPanelProvider>()
				.Select(x => x.GetPanel())
				.ToArray();

			foreach ( NavPanel navPanel in navPanels ) {
				foreach ( NavGrid navGrid in navPanel.Grids ) {
					foreach ( NavItem navItem in navGrid.Items ) {
						navItem.OnSelected += HandleItemSelected;
					}
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(NavItem pNavItem) {
			if ( pNavItem.Label == "^" ) {
				return;
			}
			
			if ( pNavItem.Label.Length == 1 ) {
				vEnviro.AddLetter(pNavItem.Label[0]);
				vTextField.AddLetter(pNavItem.Label[0]);
				return;
			}

			if ( pNavItem.Label == "Back" ) {
				vEnviro.RemoveLatestLetter();
				vTextField.RemoveLatestLetter();
			}

			if ( pNavItem.Label == "Enter" ) {
				vTextField.ClearLetters();
			}
		}

	}

}
