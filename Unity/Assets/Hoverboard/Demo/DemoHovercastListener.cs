using System.Linq;
using Hoverboard.Core;
using Hoverboard.Core.Navigation;
using Hovercast.Core;
using Hovercast.Core.State;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoHovercastListener : MonoBehaviour {

		private GameObject vTextField;
		private HoverboardSetup vHoverboardSetup;
		private HovercastSetup vHovercastSetup;
		private NavPanel[] vKeyboardNavPanels;
		private bool vPrevEnableKey;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vTextField = GameObject.Find("DemoTextField");
			vHoverboardSetup = GameObject.Find("Hoverboard").GetComponent<HoverboardSetup>();
			vHovercastSetup = GameObject.Find("Hovercast").GetComponent<HovercastSetup>();

			vKeyboardNavPanels = GameObject.Find("SplitKeyboard")
				.GetComponentsInChildren<HoverboardPanelProvider>()
				.Select(x => x.GetPanel())
				.ToArray();

			vPrevEnableKey = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			IHovercastState state = vHovercastSetup.State;
			bool enableKey = !state.IsMenuVisible;

			if ( vPrevEnableKey != enableKey ) {
				HandleEnabledChange(enableKey);
				vPrevEnableKey = enableKey;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleEnabledChange(bool pEnabled) {
			vHoverboardSetup.InputProvider.IsEnabled = pEnabled;
			vTextField.SetActive(pEnabled);

			foreach ( NavPanel navPanel in vKeyboardNavPanels ) {
				navPanel.Container.SetActive(pEnabled);

				/*foreach ( NavGrid navGrid in navPanel.Grids ) {
					foreach ( NavItem navItem in navGrid.Items ) {
						navItem.IsEnabled = pEnabled;
					}
				}*/
			}
		}

	}

}
