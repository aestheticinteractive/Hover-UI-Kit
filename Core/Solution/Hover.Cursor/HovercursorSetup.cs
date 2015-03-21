using System.Collections.Generic;
using System.Linq;
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
		private IDictionary<CursorType, UiCursor> vCursorMap;


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

			vState = new HovercursorState(gameObject.transform, InputProvider,
				DefaultVisualSettings, CameraReference);
			vComponentSuccess = true;

			vCursorMap = new Dictionary<CursorType, UiCursor>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vComponentSuccess ) {
				return;
			}

			InputProvider.UpdateInput();
			vState.UpdateAfterInput();

			CursorType[] newTypes = vState.InitializedCursorTypes;
			CursorType[] removeCursorTypes = vCursorMap.Keys.Except(newTypes).ToArray();
			CursorType[] addCursorTypes = newTypes.Except(vCursorMap.Keys).ToArray();
			ICursorSettings visualSett = DefaultVisualSettings.GetSettings();

			foreach ( CursorType cursorType in removeCursorTypes ) {
				UiCursor uiCursor = vCursorMap[cursorType];
				vCursorMap.Remove(cursorType);
				Destroy(uiCursor.gameObject);
			}

			foreach ( CursorType cursorType in addCursorTypes ) {
				ICursorState cursor = vState.GetCursorState(cursorType);

				var cursorObj = new GameObject("Cursor-"+cursorType);
				cursorObj.transform.SetParent(gameObject.transform, false);
				UiCursor uiCursor = cursorObj.AddComponent<UiCursor>();
				uiCursor.Build(cursor, visualSett, CameraReference);

				vCursorMap.Add(cursorType, uiCursor);
				vState.SetCursorTransform(cursorType, cursorObj.transform);
			}
		}

	}

}
