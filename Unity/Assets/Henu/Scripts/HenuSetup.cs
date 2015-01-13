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
		private Renderers vRenderers;
		private UiArc vUiArc;
		private UiCursor vUiCursor;


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
			vUiArc = arcObj.AddComponent<UiArc>();
			vUiArc.Build(vMenuState.Arc, vRenderers);

			var cursorObj = new GameObject("Cursor");
			cursorObj.transform.SetParent(handControlObj.transform, false);
			vUiCursor = cursorObj.AddComponent<UiCursor>();
			vUiCursor.Build(vMenuState.Arc, vMenuState.Cursor, vRenderers);
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
				PointParent = typeof(UiArcSegmentParentRenderer),
				PointSelection = typeof(UiArcSegmentRenderer),
				PointCheckbox = typeof(UiArcSegmentCheckboxRenderer),
				PointRadio = typeof(UiArcSegmentRadioRenderer)
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
