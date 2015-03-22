using System;
using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using UnityEngine;
using Hover.Cursor;
using Hover.Common.Util;

namespace Hover.Cast {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastItemsProvider NavigationProvider;
		public HovercastCustomizationProvider CustomizationProvider;
		public HovercastInputProvider InputProvider;
		public Transform OptionalCameraReference;
		public HovercursorSetup Hovercursor;
		
		private HovercastState vState;
		private bool vFailed;
		private UiMenu vUiMenu;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercastState State {
			get {
				return vState;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			const string prefix = "Hoverboard";
			
			/*DefaultItemVisualSettings = UnityUtil.CreateComponent<HoverboardItemVisualSettings,
				HoverboardItemVisualSettingsStandard>(DefaultItemVisualSettings, gameObject, prefix);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;
			
			ProjectionVisualSettings = UnityUtil.FindComponentOrCreate<
				HoverboardProjectionVisualSettings, HoverboardProjectionVisualSettingsStandard>(
					ProjectionVisualSettings,gameObject,prefix);
			
			InteractionSettings = UnityUtil.FindComponentOrCreate<HoverboardInteractionSettings,
				HoverboardInteractionSettings>(InteractionSettings, gameObject, prefix);*/

			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, prefix);

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

			vState = new HovercastState(NavigationProvider, CustomizationProvider, 
				InputProvider, Hovercursor, gameObject.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( vFailed ) {
				return;
			}

			var menuObj = new GameObject("Menu");
			menuObj.transform.SetParent(gameObject.transform, false);
			vUiMenu = menuObj.AddComponent<UiMenu>();
			vUiMenu.Build(vState, CustomizationProvider, CustomizationProvider);

			vState.SetReferences(menuObj.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vFailed ) {
				return;
			}

			InputProvider.UpdateInput();
			vState.UpdateAfterInput();
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
