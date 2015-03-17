using System;
using System.Collections.Generic;
using Hover.Board.Custom;
using Hover.Board.Input;
using Hover.Board.Navigation;
using UnityEngine;

namespace Hover.Board.State {

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
		public OverallState(IInputProvider pInputProv, IEnumerable<ItemPanel> pPanels,
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

			foreach ( ItemPanel navPanel in pPanels ) {
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
			float nearestDist = float.MaxValue;

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

				float buttonDist = button.GetHighlightDistance(pType);

				if ( buttonDist < nearestDist ) {
					nearestTree = buttonTree;
					nearestDist = buttonDist;
				}
			}

			foreach ( ButtonTree buttonTree in vAllButtons ) {
				ButtonState button = buttonTree.Button;
				button.SetAsNearestButton(pType, (button == nearestTree.Button));
			}

			vNearestButtonMap[pType] = nearestTree;

			if ( nearestTree.Panel == null || nearestTree.Button.MaxHighlightProgress <= 0 ) {
				pCursor.SetNearestPanelTransform(null);
				pCursor.NearestButtonHighlightProgress = 0;
			}
			else {
				GameObject panelObj = (GameObject)nearestTree.Panel.ItemPanel.DisplayContainer;
				pCursor.SetNearestPanelTransform(panelObj.transform);
				pCursor.NearestButtonHighlightProgress = nearestTree.Button.GetHighlightProgress(pType);
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
