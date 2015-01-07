using HandMenu.Demo;
using HandMenu.Display;
using HandMenu.Input;
using HandMenu.Navigation;
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
		private NavigationProvider vNavProv;
		private MenuState vMenuState;
		private MenuHandDisplay vMenuHandDisp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();
			vLeapControl = vHandControl.GetLeapController();

			vInputProv = new InputProvider();
			vNavProv = new NavigationProvider();
			vMenuState = new MenuState(vInputProv, vNavProv, LeftHandMenu);

			vNavProv.Init(new DemoData());

			////

			var menuHandObj = new GameObject("MenuHandDisplay");
			menuHandObj.transform.parent = handControlObj.transform;
			menuHandObj.transform.localPosition = Vector3.zero;
			menuHandObj.transform.localRotation = Quaternion.identity;
			menuHandObj.transform.localScale = Vector3.one;

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

	}

}
