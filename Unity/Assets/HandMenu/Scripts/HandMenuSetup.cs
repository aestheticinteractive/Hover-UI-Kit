using HandMenu.Demo;
using HandMenu.Display;
using HandMenu.Display.Default;
using HandMenu.Input;
using HandMenu.Navigation;
using HandMenu.State;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandMenuSetup : MonoBehaviour {

		public bool LeftHandMenu = true;
		public Component PointParentRenderer;
		public Component PointSelectionRenderer;

		private HandController vHandControl;
		private Controller vLeapControl;
		private InputProvider vInputProv;
		private NavigationProvider vNavProv;
		private MenuState vMenuState;
		private UiMenuHand vUiMenuHand;


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
			 
			var renderers = new Renderers {
				PointParent = Renderers.GetType(PointParentRenderer, typeof(UiPointParentRenderer)),
				PointSelection = Renderers.GetType(PointSelectionRenderer, typeof(UiPointRenderer)),
			};

			renderers.Verify();

			////

			var menuHandObj = new GameObject("MenuHandDisplay");
			menuHandObj.transform.parent = handControlObj.transform;
			menuHandObj.transform.localPosition = Vector3.zero;
			menuHandObj.transform.localRotation = Quaternion.identity;
			menuHandObj.transform.localScale = Vector3.one;

			vUiMenuHand = menuHandObj.AddComponent<UiMenuHand>();
			vUiMenuHand.Build(vMenuState.MenuHand, renderers);
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
