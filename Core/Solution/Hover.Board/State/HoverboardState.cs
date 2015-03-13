using System.Collections.Generic;
using Hover.Board.Custom;
using Hover.Board.Display;
using Hover.Board.Input;
using Hover.Board.Navigation;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class HoverboardState : IHoverboardState {

		public HoverboardPanelProvider[] PanelProviders { get; private set; }
		public HoverboardCustomizationProvider CustomizationProvider { get; private set; }
		public HoverboardInputProvider InputProvider { get; private set; }

		public Transform CursorRightTransform { get; private set; }
		public Transform CameraTransform { get; private set; }

		private OverallState vOverallState;
		private IDictionary<CursorType, Transform> vCursorTransformMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverboardState(HoverboardPanelProvider[] pPanels, 
								HoverboardCustomizationProvider pCustom, HoverboardInputProvider pInput,
								Transform pCamera) {
			PanelProviders = pPanels;
			CustomizationProvider = pCustom;
			InputProvider = pInput;
			CameraTransform = pCamera;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(OverallState pOverallState, 
													IDictionary<CursorType, UiCursor> pUiCursorMap) {
			vOverallState = pOverallState;
			vCursorTransformMap = new Dictionary<CursorType, Transform>();

			foreach ( KeyValuePair<CursorType, UiCursor> pair in pUiCursorMap ) {
				vCursorTransformMap.Add(pair.Key, pair.Value.transform);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Transform GetCursorTransform(CursorType pType) {
			return vCursorTransformMap[pType];
		}

	}

}
