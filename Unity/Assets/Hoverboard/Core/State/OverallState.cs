using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class OverallState {

		private struct ButtonTree {
			public PanelState Panel;
			public GridState Grid;
			public ButtonState Button;
		}

		public PanelState[] Panels { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, CursorState> vCursorMap;
		private readonly IDictionary<CursorType, ButtonTree?> vNearestButtonMap;
		private readonly ButtonTree[] vAllButtons;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public OverallState(IInputProvider pInputProv, IEnumerable<NavPanel> pPanels,
													InteractionSettings pSettings, Transform pBaseTx) {
			vInputProv = pInputProv;
			vBaseTx = pBaseTx;

			vCursorMap = new Dictionary<CursorType, CursorState>();
			vNearestButtonMap = new Dictionary<CursorType, ButtonTree?>();

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				vCursorMap.Add(cursorType, new CursorState(pSettings, vBaseTx));
				vNearestButtonMap[cursorType] = null;
			}

			////

			var panels = new List<PanelState>();
			var allButtons = new List<ButtonTree>();

			foreach ( NavPanel navPanel in pPanels ) {
				var panel = new PanelState(navPanel, pSettings);
				panels.Add(panel);

				foreach ( GridState grid in panel.Grids ) {
					foreach ( ButtonState button in grid.Buttons ) {
						var tree = new ButtonTree {
							Panel = panel,
							Grid = grid,
							Button = button
						};

						allButtons.Add(tree);
					}
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
			Vector3? cursorWorldPos = (allowSelect ? pCursor.GetWorldPosition() : (Vector3?)null);
			ButtonTree nearestTree = new ButtonTree();

			foreach ( ButtonTree buttonTree in vAllButtons ) {
				ButtonState button = buttonTree.Button;
				button.UpdateWithCursor(pType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				if ( nearestTree.Button == null ) {
					nearestTree = buttonTree;
					continue;
				}

				if ( button.GetHighlightDistance(pType) < nearestTree.Button.MinHighlightDistance ) {
					nearestTree = buttonTree;
				}
			}

			foreach ( ButtonTree buttonTree in vAllButtons ) {
				ButtonState button = buttonTree.Button;
				button.SetAsNearestButton(pType, (button == nearestTree.Button));
			}

			vNearestButtonMap[pType] = nearestTree;

			if ( nearestTree.Panel == null || nearestTree.Button.MaxHighlightProgress <= 0 ) {
				pCursor.SetNearestPanelTransform(null);
			}
			else {
				pCursor.SetNearestPanelTransform(nearestTree.Panel.NavPanel.Container.transform);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ButtonTree buttonTree in vAllButtons ) {
				buttonTree.Button.UpdateSelectionProcess();
			}
		}

	}

}
