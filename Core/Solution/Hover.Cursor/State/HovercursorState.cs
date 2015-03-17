using System.Collections.Generic;
using Hover.Cursor.Custom;
using Hover.Cursor.Display;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class HovercursorState : IHovercursorState {

		public HovercursorCustomizationProvider CustomizationProvider { get; private set; }
		public HovercursorInputProvider InputProvider { get; private set; }

		public Transform CameraTransform { get; private set; }

		private IDictionary<CursorType, CursorState> vCursorStateMap;
		private IDictionary<CursorType, Transform> vCursorTransformMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorState(HovercursorCustomizationProvider pCustom,
												HovercursorInputProvider pInput, Transform pCamera) {
			CustomizationProvider = pCustom;
			InputProvider = pInput;
			CameraTransform = pCamera;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(IDictionary<CursorType, CursorState> pCursorStateMap, 
													IDictionary<CursorType, UiCursor> pUiCursorMap) {
			vCursorStateMap = pCursorStateMap;
			vCursorTransformMap = new Dictionary<CursorType, Transform>();

			foreach ( KeyValuePair<CursorType, UiCursor> pair in pUiCursorMap ) {
				vCursorTransformMap.Add(pair.Key, pair.Value.transform);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public ICursorState GetCursorState(CursorType pType) {
			return vCursorStateMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public Transform GetCursorTransform(CursorType pType) {
			return vCursorTransformMap[pType];
		}

	}

}
