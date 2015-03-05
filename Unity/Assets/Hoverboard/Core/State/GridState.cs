using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class GridState {

		public NavGrid NavGrid { get; private set; }
		public ButtonState[] Buttons { get; private set; }

		private readonly InteractionSettings vSettings;
		private readonly IDictionary<CursorType, ButtonState> vNearestButtonMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public GridState(NavGrid pNavGrid, InteractionSettings pSettings) {
			NavGrid = pNavGrid;
			vSettings = pSettings;
			vNearestButtonMap = new Dictionary<CursorType, ButtonState>();

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				vNearestButtonMap[cursorType] = null;
			}

			RefreshButtons();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursors(IDictionary<CursorType, CursorState> pCursorMap) {
			foreach ( KeyValuePair<CursorType, CursorState> pair in pCursorMap ) {
				UpdateWithCursor(pair.Key, pair.Value);
			}

			UpdateAfterAllCursors();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshButtons() {
			Buttons = new ButtonState[NavGrid.Items.Length];

			for ( int i = 0 ; i < NavGrid.Items.Length ; i++ ) {
				var button = new ButtonState(NavGrid.Items[i], vSettings);
				Buttons[i] = button;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithCursor(CursorType pType, CursorState pCursor) {
			bool allowSelect = (pCursor.IsInputAvailable && NavGrid.IsVisible);
			Vector3? cursorPos = (allowSelect ? pCursor.Position : (Vector3?)null);

			vNearestButtonMap[pType] = null;

			foreach ( ButtonState button in Buttons ) {
				button.UpdateWithCursor(pType, cursorPos);

				if ( !allowSelect ) {
					continue;
				}

				if ( vNearestButtonMap[pType] == null ) {
					vNearestButtonMap[pType] = button;
					continue;
				}

				if ( button.MinHighlightDistance < vNearestButtonMap[pType].MinHighlightDistance ) {
					vNearestButtonMap[pType] = button;
				}
			}

			foreach ( ButtonState button in Buttons ) {
				button.SetAsNearestButton(pType, (button == vNearestButtonMap[pType]));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ButtonState button in Buttons ) {
				button.UpdateSelectionProcess();
			}
		}

	}

}
