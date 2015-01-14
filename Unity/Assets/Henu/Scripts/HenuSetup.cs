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
		public bool TestMode;
		public HenuNavComponent NavDelegateProvider;
		public Component ArcSegmentParentRenderer;
		public Component ArcSegmentSelectionRenderer;
		public Component ArcSegmentCheckboxRenderer;
		public Component ArcSegmentRadioRenderer;
		public Component CursorRenderer;

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
			vMenuState = new MenuState(GetInputProv(), vNavProv, MenuIsOnLeftHand);

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
			if ( OVRManager.capiHmd != null && OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				OVRManager.capiHmd.DismissHSWDisplay();
			}

			vInputProv.UpdateWithLeapFrame(vHandControl.GetLeapController().Frame());
			vMenuState.UpdateAfterInput();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildRenderers() {
			vRenderers = new Renderers {
				ArcSegmentParent = typeof(UiArcSegmentParentRenderer),
				ArcSegmentSelection = typeof(UiArcSegmentRenderer),
				ArcSegmentCheckbox = typeof(UiArcSegmentCheckboxRenderer),
				ArcSegmentRadio = typeof(UiArcSegmentRadioRenderer),
				Cursor = typeof(UiCursorRenderer)
			};

			if ( ArcSegmentParentRenderer != null ) {
				vRenderers.ArcSegmentParent = ArcSegmentParentRenderer.GetType();
			}

			if ( ArcSegmentSelectionRenderer != null ) {
				vRenderers.ArcSegmentSelection = ArcSegmentSelectionRenderer.GetType();
			}

			if ( ArcSegmentCheckboxRenderer != null ) {
				vRenderers.ArcSegmentCheckbox = ArcSegmentCheckboxRenderer.GetType();
			}

			if ( ArcSegmentRadioRenderer != null ) {
				vRenderers.ArcSegmentRadio = ArcSegmentRadioRenderer.GetType();
			}

			if ( CursorRenderer != null ) {
				vRenderers.Cursor = CursorRenderer.GetType();
			}

			vRenderers.Verify();
		}

		/*--------------------------------------------------------------------------------------------*/
		private IInputProvider GetInputProv() {
			if ( !TestMode ) {
				return vInputProv;
			}

			GameObject obj = GameObject.Find("TestInput");
			int count = obj.transform.childCount;

			for ( int i = 0 ; i < count ; ++i ) {
				object comp = obj.GetComponent(typeof(IInputProvider));

				if ( comp != null ) {
					return (IInputProvider)comp;
				}
			}

			return null;
		}

	}

}
