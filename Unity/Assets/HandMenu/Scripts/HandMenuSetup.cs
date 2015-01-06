using HandMenu.Display;
using HandMenu.Input;
using HandMenu.State;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandMenuSetup : MonoBehaviour {

		public bool LeftHandMenu = true;

		private HandController vHandControl;
		private Controller vLeapControl;
		private InputProvider vInputProv;
		private MenuState vMenuState;
		private MenuHandDisplay vMenuHandDisp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();
			vLeapControl = vHandControl.GetLeapController();

			vInputProv = new InputProvider();
			vMenuState = new MenuState(vInputProv, LeftHandMenu);

			////

			var menuHandObj = new GameObject("MenuHandDisplay");
			SetAndMoveToParent(menuHandObj.transform, handControlObj.transform);

			vMenuHandDisp = menuHandObj.AddComponent<MenuHandDisplay>();
			vMenuHandDisp.MenuHand = vMenuState.MenuHand;
			vMenuHandDisp.SelectHand = vMenuState.SelectHand;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				OVRManager.capiHmd.DismissHSWDisplay();
			}

			vInputProv.UpdateWithFrame(vLeapControl.Frame());
			vMenuState.UpdateAfterInput();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void SetAndMoveToParent(Transform pChild, Transform pParent) {
			pChild.parent = pParent;
			pChild.position = pParent.position;
			pChild.rotation = pParent.rotation;
			pChild.localScale = Vector3.one;
		}

	}

}
