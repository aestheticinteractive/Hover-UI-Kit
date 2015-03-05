using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class OverallState {

		public PanelState[] Panels { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly IDictionary<CursorType, CursorState> vCursorMap;
		private readonly IDictionary<CursorType, ButtonState> vNearestButtonMap;
		private readonly ButtonState[] vAllButtons;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public OverallState(IInputProvider pInputProv, IEnumerable<NavPanel> pPanels,
																		InteractionSettings pSettings) {
			vInputProv = pInputProv;

			vCursorMap = new Dictionary<CursorType, CursorState>();
			vNearestButtonMap = new Dictionary<CursorType, ButtonState>();

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				vCursorMap.Add(cursorType, new CursorState(pSettings));
				vNearestButtonMap[cursorType] = null;
			}

			////

			var panels = new List<PanelState>();
			var allButtons = new List<ButtonState>();

			foreach ( NavPanel navPanel in pPanels ) {
				var panelState = new PanelState(navPanel, pSettings);
				panels.Add(panelState);

				foreach ( GridState grid in panelState.Grids ) {
					allButtons.AddRange(grid.Buttons);
				}
			}

			Panels = panels.ToArray();
			vAllButtons = allButtons.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState GetCursor(CursorType pType) {
			return vCursorMap[pType];
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			foreach ( KeyValuePair<CursorType, CursorState> pair in vCursorMap ) {
				pair.Value.UpdateAfterInput(vInputProv.GetCursor(pair.Key));
				UpdateWithCursor(pair.Key, pair.Value);
			}

			UpdateAfterAllCursors();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithCursor(CursorType pType, CursorState pCursor) {
			bool allowSelect = (pCursor.IsInputAvailable); //TODO: && NavGrid.IsVisible);
			Vector3? cursorPos = (allowSelect ? pCursor.Position : (Vector3?)null);

			//TODO: allow a "nearest" for each cursor
			vNearestButtonMap[pType] = null;

			foreach ( ButtonState button in vAllButtons ) {
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

			foreach ( ButtonState button in vAllButtons ) {
				button.SetAsNearestButton(pType, (button == vNearestButtonMap[pType]));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ButtonState button in vAllButtons ) {
				button.UpdateSelectionProcess();
			}
		}

	}

}
