using System;
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
		public HandMenuNavComponent NavDelegateProvider;
		public Component PointParentRenderer;
		public Component PointSelectionRenderer;
		public Component PointCheckboxRenderer;
		public Component PointRadioRenderer;

		private HandController vHandControl;
		private Controller vLeapControl;
		private InputProvider vInputProv;
		private NavigationProvider vNavProv;
		private MenuState vMenuState;
		private UiMenuHand vUiMenuHand;
		private Renderers vRenderers;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();
			vLeapControl = vHandControl.GetLeapController();

			vInputProv = new InputProvider();
			vNavProv = new NavigationProvider();
			vMenuState = new MenuState(vInputProv, vNavProv, LeftHandMenu);

			if ( NavDelegateProvider == null ) {
				throw new Exception("No menu delegate was provided!");
			}

			vNavProv.Init(NavDelegateProvider.GetNavDelegate());

			////

			BuildRenderers();

			var menuHandObj = new GameObject("MenuHand");
			menuHandObj.transform.parent = handControlObj.transform;
			menuHandObj.transform.localPosition = Vector3.zero;
			menuHandObj.transform.localRotation = Quaternion.identity;
			menuHandObj.transform.localScale = Vector3.one;

			vUiMenuHand = menuHandObj.AddComponent<UiMenuHand>();
			vUiMenuHand.Build(vMenuState.MenuHand, vRenderers);
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
		private void BuildRenderers() {
			vRenderers = new Renderers {
				PointParent = typeof(UiPointParentRenderer),
				PointSelection = typeof(UiPointRenderer),
				PointCheckbox = typeof(UiPointCheckboxRenderer),
				PointRadio = typeof(UiPointRadioRenderer)
			};

			if ( PointParentRenderer != null ) {
				vRenderers.PointParent = PointParentRenderer.GetType();
			}

			if ( PointSelectionRenderer != null ) {
				vRenderers.PointSelection = PointSelectionRenderer.GetType();
			}

			if ( PointCheckboxRenderer != null ) {
				vRenderers.PointCheckbox = PointCheckboxRenderer.GetType();
			}

			if ( PointRadioRenderer != null ) {
				vRenderers.PointRadio = PointRadioRenderer.GetType();
			}

			vRenderers.Verify();
		}

	}

}
