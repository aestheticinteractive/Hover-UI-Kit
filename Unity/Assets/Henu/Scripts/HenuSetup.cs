using System;
using Henu.Display;
using Henu.Display.Default;
using Henu.Input;
using Henu.Navigation;
using Henu.State;
using UnityEngine;

namespace Henu {

	/*================================================================================================*/
	public class HenuSetup : MonoBehaviour {

		public bool MenuIsOnLeftHand = true;
		public HenuNavComponent NavDelegateProvider;
		public Component PointParentRenderer;
		public Component PointSelectionRenderer;
		public Component PointCheckboxRenderer;
		public Component PointRadioRenderer;

		private HandController vHandControl;
		private InputProvider vInputProv;
		private NavigationProvider vNavProv;
		private MenuState vMenuState;
		private UiArc vUiArc;
		private Renderers vRenderers;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();

			if ( NavDelegateProvider == null ) {
				throw new Exception("No menu delegate was provided!");
			}

			vNavProv = new NavigationProvider();
			vNavProv.Init(NavDelegateProvider.GetNavDelegate());

			vInputProv = new InputProvider();
			vMenuState = new MenuState(vInputProv, vNavProv, MenuIsOnLeftHand);

			////

			BuildRenderers();

			var arcObj = new GameObject("Arc");
			arcObj.transform.SetParent(handControlObj.transform, false);
			arcObj.transform.localPosition = Vector3.zero;
			arcObj.transform.localRotation = Quaternion.identity;
			arcObj.transform.localScale = Vector3.one;

			vUiArc = arcObj.AddComponent<UiArc>();
			vUiArc.Build(vMenuState.Arc, vRenderers);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				OVRManager.capiHmd.DismissHSWDisplay();
			}

			vInputProv.UpdateWithLeapFrame(vHandControl.GetLeapController().Frame());
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
