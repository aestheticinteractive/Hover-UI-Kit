using System;
using Hovercast.Core.Custom;
using Hovercast.Core.Display;
using Hovercast.Core.Input;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastNavProvider NavigationProvider;
		public HovercastCustomizationProvider CustomizationProvider;
		public HovercastInputProvider InputProvider;
		public Transform OptionalCameraReference;

		private bool vFailed;
		private MenuState vMenuState;
		private UiMenu vUiMenu;
		private UiCursor vUiCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( NavigationProvider == null ) {
				throw FailMissing("Navigation Provider");
			}

			if ( CustomizationProvider == null ) {
				throw FailMissing("Customization Provider");
			}

			if ( InputProvider == null ) {
				throw FailMissing("Input Provider");
			}

			if ( OptionalCameraReference == null ) {
				OptionalCameraReference = gameObject.transform;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( vFailed ) {
				return;
			}

			vMenuState = new MenuState(InputProvider, NavigationProvider.GetRoot(),
				CustomizationProvider.GetInteractionSettings());

			var menuObj = new GameObject("Menu");
			menuObj.transform.SetParent(gameObject.transform, false);
			vUiMenu = menuObj.AddComponent<UiMenu>();
			vUiMenu.Build(vMenuState, CustomizationProvider, CustomizationProvider);

			var cursorObj = new GameObject("Cursor");
			cursorObj.transform.SetParent(gameObject.transform, false);
			vUiCursor = cursorObj.AddComponent<UiCursor>();
			vUiCursor.Build(vMenuState.Arc, vMenuState.Cursor,
				CustomizationProvider, OptionalCameraReference);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vFailed ) {
				return;
			}

			InputProvider.UpdateInput();
			vMenuState.UpdateAfterInput();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Exception FailMissing(string pName) {
			vFailed = true;
			gameObject.SetActive(false);
			return new Exception("Hovercast | '"+pName+"' must be set.");
		}

	}

}
