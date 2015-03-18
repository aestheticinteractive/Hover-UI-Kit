using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.Custom.Default;
using Hover.Cursor.Display;
using Hover.Cursor.Input;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor {

	/*================================================================================================*/
	public class HovercursorSetup : MonoBehaviour {

		public HovercursorCustomizationProvider CustomizationProvider;
		public HovercursorInputProvider InputProvider;
		public Transform CameraReference;

		private HovercursorState vHoverState;
		private bool vComponentSuccess;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorState State {
			get {
				return vHoverState;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			const string prefix = "Hovercursor";

			CustomizationProvider = UnityUtil.FindComponentOrCreate<HovercursorCustomizationProvider, 
				HovercursorDefaultCustomizationProvider>(CustomizationProvider, gameObject, prefix);

			InputProvider = UnityUtil.FindComponentOrFail(InputProvider, gameObject, prefix);

			if ( CameraReference == null ) {
				CameraReference = gameObject.transform;
			}

			vHoverState = new HovercursorState(InitCursorType, InputProvider,
				CustomizationProvider, CameraReference);
			vComponentSuccess = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vComponentSuccess ) {
				return;
			}

			InputProvider.UpdateInput();
			vHoverState.UpdateAfterInput();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HovercursorState.CursorPair InitCursorType(CursorType pType) {
			var cursorState = new CursorState(InputProvider.GetCursor(pType),
				CustomizationProvider.GetSettings(), gameObject.transform);

			var cursorObj = new GameObject("Cursor"+pType);
			cursorObj.transform.SetParent(gameObject.transform, false);
			var uiCursor = cursorObj.AddComponent<UiCursor>();
			uiCursor.Build(cursorState, CustomizationProvider.GetSettings(), CameraReference);

			return new HovercursorState.CursorPair {
				State = cursorState,
				Display = uiCursor
			};
		}

	}

}
