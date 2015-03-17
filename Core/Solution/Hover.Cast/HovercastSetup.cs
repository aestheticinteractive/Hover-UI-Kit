using System;
using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastItemsProvider NavigationProvider;
		public HovercastCustomizationProvider CustomizationProvider;
		public HovercastInputProvider InputProvider;
		public Transform OptionalCameraReference;

		public IHovercastState State { get; private set; }

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

			State = new HovercastState(NavigationProvider, CustomizationProvider, 
				InputProvider, OptionalCameraReference);
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

			((HovercastState)State).SetReferences(vMenuState, menuObj.transform, cursorObj.transform);
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
