using System.Collections.Generic;
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
		private IDictionary<CursorType, CursorState> vCursorStateMap;


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

			vHoverState = new HovercursorState(CustomizationProvider, InputProvider, CameraReference);
			vComponentSuccess = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( !vComponentSuccess ) {
				return;
			}

			CursorType[] cursorTypes = InputProvider.GetAllCursorTypes();

			vCursorStateMap = new Dictionary<CursorType, CursorState>();
			var uiCursorMap = new Dictionary<CursorType, UiCursor>();

			foreach ( CursorType cursorType in cursorTypes ) {
				var cursorState = new CursorState(InputProvider.GetCursor(cursorType),
					CustomizationProvider.GetSettings(), gameObject.transform);
				vCursorStateMap[cursorType] = cursorState;

				var cursorObj = new GameObject("Cursor"+cursorType);
				cursorObj.transform.SetParent(gameObject.transform, false);
				var uiCursor = cursorObj.AddComponent<UiCursor>();
				uiCursor.Build(cursorState, CustomizationProvider.GetSettings(), CameraReference);
				uiCursorMap.Add(cursorType, uiCursor);
			}

			vHoverState.SetReferences(vCursorStateMap, uiCursorMap);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vComponentSuccess ) {
				return;
			}

			InputProvider.UpdateInput();

			foreach ( CursorState cursorState in vCursorStateMap.Values ) {
				cursorState.UpdateAfterInput();
			}
		}

	}

}
