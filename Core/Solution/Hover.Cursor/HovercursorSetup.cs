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
		public HovercursorInput Input;
		public Transform CameraTransform;

		private HovercursorState vState;
		private CursorType[] vPrevActiveCursorTypes;
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

			Input = UnityUtil.FindComponentOrFail(Input, prefix);

			if ( CameraTransform == null ) {
				CameraTransform = gameObject.transform;
			}

			vState = new HovercursorState(gameObject.transform, Input,
				DefaultVisualSettings, CameraTransform);

			vPrevActiveCursorTypes = new CursorType[0];
			vCursorMap = new Dictionary<CursorType, UiCursor>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vState == null || Input.IsFailure ) {
				return;
			}

			vState.UpdateBeforeInput();
			Input.UpdateInput();
			vState.UpdateAfterInput();

			CursorType[] activeTypes = vState.ActiveCursorTypes;
			IEnumerable<CursorType> hideTypes = vPrevActiveCursorTypes.Except(activeTypes);
			IEnumerable<CursorType> showTypes = activeTypes.Except(vPrevActiveCursorTypes);
			ICursorSettings visualSett = DefaultVisualSettings.GetSettings();

			foreach ( CursorType type in hideTypes ) {
				vCursorMap[type].gameObject.SetActive(false);
			}

			foreach ( CursorType type in showTypes ) {
				if ( vCursorMap.ContainsKey(type) ) {
					vCursorMap[type].gameObject.SetActive(true);
					continue;
				}

				ICursorState cursor = vState.GetCursorState(type);

				var cursorObj = new GameObject("Cursor-"+type);
				cursorObj.transform.SetParent(gameObject.transform, false);
				UiCursor uiCursor = cursorObj.AddComponent<UiCursor>();
				uiCursor.Build(cursor, visualSett, CameraTransform);

				vCursorMap.Add(type, uiCursor);
				vState.SetCursorTransform(type, cursorObj.transform);
			}

			vPrevActiveCursorTypes = activeTypes;
		}

	}

}
