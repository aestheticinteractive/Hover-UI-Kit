using System;
using Hovercast.Core.Display;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastNavProvider NavigationProvider;
		public HovercastSettingsComponent SettingsProvider;
		public HovercastInputProvider InputProvider;
		public Transform OptionalCameraReference;

		private MenuState vMenuState;
		private UiMenu vUiMenu;
		private UiCursor vUiCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( NavigationProvider == null ) {
				throw new Exception("HovercastSetup.NavigationProvider must be set.");
			}

			if ( SettingsProvider == null ) {
				throw new Exception("HovercastSetup.SettingsProvider must be set.");
			}

			if ( InputProvider == null ) {
				throw new Exception("HovercastSetup.InputProvider must be set.");
			}

			if ( OptionalCameraReference == null ) {
				OptionalCameraReference = gameObject.transform;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vMenuState = new MenuState(InputProvider, NavigationProvider.GetRoot(), 
				SettingsProvider.GetInteractionSettings());

			////

			var menuObj = new GameObject("Menu");
			menuObj.transform.SetParent(gameObject.transform, false);
			vUiMenu = menuObj.AddComponent<UiMenu>();
			vUiMenu.Build(vMenuState, SettingsProvider);

			var cursorObj = new GameObject("Cursor");
			cursorObj.transform.SetParent(gameObject.transform, false);
			vUiCursor = cursorObj.AddComponent<UiCursor>();
			vUiCursor.Build(vMenuState.Arc, vMenuState.Cursor, 
				SettingsProvider, OptionalCameraReference);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			InputProvider.UpdateInput();
			vMenuState.UpdateAfterInput();
		}

	}

}
