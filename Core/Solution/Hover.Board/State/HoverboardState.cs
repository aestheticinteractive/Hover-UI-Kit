using System.Collections.Generic;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor;
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

		public PanelState[] Panels { get; private set; }

		private readonly HovercursorSetup vHovercusorSetup;
		private readonly InteractionSettings vInteractSett;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, ProjectionState> vProjectionMap;
		private readonly ItemTree[] vAllItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverboardState(ItemPanel[] pItemPanels, HovercursorSetup pHovercusorSetup,
													InteractionSettings pInterSett, Transform pBaseTx) {
			vInteractSett = pInterSett;
			vHovercusorSetup = pHovercusorSetup;
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
		public IHovercursorState Hovercursor {
			get {
				return vHovercusorSetup.State;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public ProjectionState GetProjection(CursorType pCursorType) {
			if ( !vProjectionMap.ContainsKey(pCursorType) ) {
				ICursorState state = Hovercursor.GetCursorState(pCursorType);
				var proj = new ProjectionState(state, vInteractSett, vBaseTx);
				vProjectionMap.Add(pCursorType, proj);
			}

			return vProjectionMap[pCursorType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveProjection(CursorType pCursorType) {
			ProjectionState proj = vProjectionMap[pCursorType];
			proj.RemoveInteraction();
			vProjectionMap.Remove(pCursorType);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			foreach ( ProjectionState proj in vProjectionMap.Values ) {
				UpdateWithCursor(proj);
			}

			UpdateAfterAllCursors();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithCursor(ProjectionState pProj) {
			ICursorState cursor = pProj.Cursor;
			CursorType cursorType = cursor.Type;
			bool allowSelect = (cursor.IsInputAvailable); //TODO: && Grid.IsVisible);
			Vector3? cursorWorldPos = (allowSelect ? cursor.GetWorldPosition() : (Vector3?)null);
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
				pProj.SetNearestPanelTransform(null);
				pProj.NearestItemHighlightProgress = 0;
				return;
			}

			GameObject panelObj = (GameObject)nearestTree.Panel.ItemPanel.DisplayContainer;
			pProj.SetNearestPanelTransform(panelObj.transform);
			pProj.NearestItemHighlightProgress = nearestTree.Item.GetHighlightProgress(cursorType);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ItemTree itemTree in vAllItems ) {
				itemTree.Item.UpdateSelectionProcess();
			}
		}

	}

}
