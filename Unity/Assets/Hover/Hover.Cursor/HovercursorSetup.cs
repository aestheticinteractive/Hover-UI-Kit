using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public Camera CenterCamera;

		private HovercursorState vState;
		private List<CursorType> vPrevActiveCursorTypes;
		private IDictionary<CursorType, UiCursor> vCursorMap;
		private List<CursorType> vHideCursorTypes;
		private List<CursorType> vShowCursorTypes;


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
			CenterCamera = UnityUtil.FindComponentOrFail(CenterCamera, prefix);

			vState = new HovercursorState(Input, DefaultVisualSettings,
				CenterCamera.gameObject.transform);

			vPrevActiveCursorTypes = new List<CursorType>();
			vCursorMap = new Dictionary<CursorType, UiCursor>(EnumIntKeyComparer.CursorType);
			vHideCursorTypes = new List<CursorType>();
			vShowCursorTypes = new List<CursorType>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vState == null || Input.IsFailure ) {
				return;
			}

			vState.UpdateBeforeInput();
			Input.UpdateInput();
			vState.UpdateAfterInput();

			ReadOnlyCollection<CursorType> activeTypes = vState.ActiveCursorTypes;
			ICursorSettings visualSett = DefaultVisualSettings.GetSettings();
			Transform cameraTx = CenterCamera.gameObject.transform;

			CursorTypeUtil.Exclude(vPrevActiveCursorTypes, activeTypes, vHideCursorTypes);
			CursorTypeUtil.Exclude(activeTypes, vPrevActiveCursorTypes, vShowCursorTypes);
			
			foreach ( CursorType type in vHideCursorTypes ) {
				vCursorMap[type].gameObject.SetActive(false);
			}

			foreach ( CursorType type in vShowCursorTypes ) {
				if ( vCursorMap.ContainsKey(type) ) {
					vCursorMap[type].gameObject.SetActive(true);
					continue;
				}

				ICursorState cursor = vState.GetCursorState(type);

				var cursorObj = new GameObject("Cursor-"+type);
				cursorObj.transform.SetParent(gameObject.transform, false);
				UiCursor uiCursor = cursorObj.AddComponent<UiCursor>();
				uiCursor.Build(cursor, visualSett, cameraTx);

				vCursorMap.Add(type, uiCursor);
				vState.SetCursorTransform(type, cursorObj.transform);
			}

			vPrevActiveCursorTypes.Clear();
			vPrevActiveCursorTypes.AddRange(activeTypes);
		}

	}

}
