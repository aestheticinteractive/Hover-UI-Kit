using System;
using Henu.Display;
using Henu.Input;
using Henu.Navigation;
using Henu.State;
using UnityEngine;

namespace Henu {

	/*================================================================================================*/
	public class HenuSetup : MonoBehaviour {

		public Vector3 PalmDirection = Vector3.down;
		public bool TestMode;
		public HenuNavComponent NavDelegateProvider;
		public HenuSettingsComponent SettingsProvider;

		private HandController vHandControl;
		private InputProvider vInputProv;
		private NavigationProvider vNavProv;
		private MenuState vMenuState;
		private UiMenu vUiMenu;
		private UiCursor vUiCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();

			if ( NavDelegateProvider == null ) {
				throw new Exception("NavDelegateProvider must be set.");
			}

			if ( SettingsProvider == null ) {
				throw new Exception("SettingsProvider must be set.");
			}

			vNavProv = new NavigationProvider();
			vNavProv.Init(NavDelegateProvider.GetNavDelegate());

			vInputProv = new InputProvider(PalmDirection);
			vMenuState = new MenuState(GetInputProv(), vNavProv,
				SettingsProvider.GetInteractionSettings());

			////

			var menuObj = new GameObject("Menu");
			menuObj.transform.SetParent(handControlObj.transform, false);
			vUiMenu = menuObj.AddComponent<UiMenu>();
			vUiMenu.Build(vMenuState, SettingsProvider);

			var cursorObj = new GameObject("Cursor");
			cursorObj.transform.SetParent(handControlObj.transform, false);
			vUiCursor = cursorObj.AddComponent<UiCursor>();
			vUiCursor.Build(vMenuState.Arc, vMenuState.Cursor, SettingsProvider);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vInputProv.UpdateWithLeapFrame(vHandControl.GetLeapController().Frame());
			vMenuState.UpdateAfterInput();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
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
