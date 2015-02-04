using System;
using Hovercast.Core.Display;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastNavComponent NavDelegateProvider;
		public HovercastSettingsComponent SettingsProvider;
		public HovercastInputProvider InputProvider;
		public Transform OptionalCameraReference;

		private NavProvider vNavProv;
		private MenuState vMenuState;
		private UiMenu vUiMenu;
		private UiCursor vUiCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( NavDelegateProvider == null ) {
				throw new Exception("HovercastSetup.NavDelegateProvider must be set.");
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

			vNavProv = new NavProvider();
			vNavProv.Init(NavDelegateProvider.GetNavDelegate());

			vMenuState = new MenuState(InputProvider, vNavProv, 
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
