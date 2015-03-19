using System.Collections.Generic;
using Hover.Board.Navigation;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class HoverboardState : IHoverboardState {

		private struct ItemTree {
			public PanelState Panel;
			public GridState Grid;
			public BaseItemState Item;
		}

		public IHovercursorState HovercursorState { get; private set; }

		public PanelState[] Panels { get; private set; }

		private readonly InteractionSettings vInteractSett;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, ProjectionState> vProjectionMap;
		private readonly ItemTree[] vAllItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverboardState(InteractionSettings pInterSett, ItemPanel[] pItemPanels, 
																					Transform pBaseTx) {
			vInteractSett = pInterSett;

			vBaseTx = pBaseTx;
			vProjectionMap = new Dictionary<CursorType, ProjectionState>();

			////

			var panels = new List<PanelState>();
			var allItems = new List<ItemTree>();

			foreach ( ItemPanel itemPanel in pItemPanels ) {
				var panel = new PanelState(itemPanel, vInteractSett);
				panels.Add(panel);

				foreach ( GridState grid in panel.Grids ) {
					foreach ( BaseItemState item in grid.Items ) {
						var tree = new ItemTree {
							Panel = panel,
							Grid = grid,
							Item = item
						};

						allItems.Add(tree);
					}
				}
			}

			Panels = panels.ToArray();
			vAllItems = allItems.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetHovercursorState(IHovercursorState pHovercursorState) {
			HovercursorState = pHovercursorState;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public ProjectionState GetProjectionState(CursorType pCursorType) {
			if ( !vProjectionMap.ContainsKey(pCursorType) ) {
				ICursorState cursorState = HovercursorState.GetCursorState(pCursorType);
				var projState = new ProjectionState(cursorState, vInteractSett, vBaseTx);
				vProjectionMap.Add(pCursorType, projState);
			}

			return vProjectionMap[pCursorType];
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			foreach ( ProjectionState projState in vProjectionMap.Values ) {
				UpdateWithCursor(projState);
			}

			UpdateAfterAllCursors();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithCursor(ProjectionState pProjection) {
			ICursorState cursorState = pProjection.CursorState;
			CursorType cursorType = cursorState.Type;
			bool allowSelect = (cursorState.IsInputAvailable); //TODO: && NavGrid.IsVisible);
			Vector3? cursorWorldPos = (allowSelect ? cursorState.GetWorldPosition() : (Vector3?)null);
			ItemTree nearestTree = new ItemTree();
			float nearestDist = float.MaxValue;

			foreach ( ItemTree itemTree in vAllItems ) {
				BaseItemState item = itemTree.Item;
				item.UpdateWithCursor(cursorType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				float itemDist = item.GetHighlightDistance(cursorType);

				if ( itemDist >= nearestDist ) {
					continue;
				}

				nearestTree = itemTree;
				nearestDist = itemDist;
			}

			foreach ( ItemTree itemTree in vAllItems ) {
				BaseItemState item = itemTree.Item;
				item.SetAsNearestItem(cursorType, (item == nearestTree.Item));
			}

			if ( nearestTree.Panel == null || nearestTree.Item.MaxHighlightProgress <= 0 ) {
				pProjection.SetNearestPanelTransform(null);
				pProjection.NearestItemHighlightProgress = 0;
				return;
			}

			GameObject panelObj = (GameObject)nearestTree.Panel.ItemPanel.DisplayContainer;
			pProjection.SetNearestPanelTransform(panelObj.transform);
			pProjection.NearestItemHighlightProgress = 
				nearestTree.Item.GetHighlightProgress(cursorType);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ItemTree itemTree in vAllItems ) {
				itemTree.Item.UpdateSelectionProcess();
			}
		}

	}

}
