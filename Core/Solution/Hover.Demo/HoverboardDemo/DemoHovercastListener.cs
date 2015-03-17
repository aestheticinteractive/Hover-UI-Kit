using System.Linq;
using Hover.Board;
using Hover.Board.Navigation;
using Hover.Cast;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Demo.HoverboardDemo {

	/*================================================================================================*/
	public class DemoHovercastListener : MonoBehaviour {

		private GameObject vTextField;
		private HoverboardSetup vHoverboardSetup;
		private HovercastSetup vHovercastSetup;
		private ItemPanel[] vKeyboardItemPanels;
		private bool vPrevEnableKey;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vTextField = GameObject.Find("DemoTextField");
			vHoverboardSetup = GameObject.Find("Hoverboard").GetComponent<HoverboardSetup>();
			vHovercastSetup = GameObject.Find("Hovercast").GetComponent<HovercastSetup>();

			vKeyboardItemPanels = GameObject.Find("SplitKeyboard")
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

			foreach ( ItemPanel navPanel in vKeyboardItemPanels ) {
				navPanel.DisplayContainer.SetActive(pEnabled);

				/*foreach ( NavGrid navGrid in navPanel.Grids ) {
					foreach ( NavItem navItem in navGrid.Items ) {
						navItem.IsEnabled = pEnabled;
					}
				}*/
			}
		}

	}

}
