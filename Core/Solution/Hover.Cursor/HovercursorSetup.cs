using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.Custom.Standard;
using Hover.Cursor.Display;
using Hover.Cursor.Input;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor {

	/*================================================================================================*/
	public class HovercursorSetup : MonoBehaviour {

		public HovercursorVisualSettings DefaultVisualSettings;
		public HovercursorInputProvider InputProvider;
		public Transform CameraReference;

		private HovercursorState vState;
		private bool vComponentSuccess;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorState State {
			get {
				return vState;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			const string prefix = "Hovercursor";

			DefaultVisualSettings = UnityUtil.FindComponentOrCreate<HovercursorVisualSettings, 
				HovercursorVisualSettingsStandard>(DefaultVisualSettings, gameObject, prefix);

			InputProvider = UnityUtil.FindComponentOrFail(InputProvider, prefix);

			if ( CameraReference == null ) {
				CameraReference = gameObject.transform;
			}

			vState = new HovercursorState(InitCursorType, InputProvider,
				DefaultVisualSettings, CameraReference);
			vComponentSuccess = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vComponentSuccess ) {
				return;
			}

			InputProvider.UpdateInput();
			vState.UpdateAfterInput();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HovercursorState.CursorPair InitCursorType(CursorType pType) {
			CursorSettings visualSett = DefaultVisualSettings.GetSettings();

			var cursorState = new CursorState(InputProvider.GetCursor(pType), 
				visualSett, gameObject.transform);

			var cursorObj = new GameObject("Cursor-"+pType);
			cursorObj.transform.SetParent(gameObject.transform, false);
			var uiCursor = cursorObj.AddComponent<UiCursor>();
			uiCursor.Build(cursorState, visualSett, CameraReference);

			return new HovercursorState.CursorPair {
				State = cursorState,
				Display = uiCursor
			};
		}

	}

}
