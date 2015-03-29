using System.Collections.Generic;
using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class HoverboardState : IHoverboardState, IHovercursorDelegate {

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

		private IBaseItemInteractionState[] vActiveCursorInteractions;
		private PlaneData[] vActiveCursorPlanes;


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
				panel.InteractionPlane = new PlaneData("Hoverboard.Panel-"+panels.Count, 
					((GameObject)panel.ItemPanel.DisplayContainer).transform, Vector3.up);
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
			ActiveCursorTypes = new CursorType[0];
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
		public void ActivateProjection(CursorType pCursorType, bool pIsActive) {
			vProjectionMap[pCursorType].IsActive = pIsActive;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			PanelState[] activePanels = Panels
				.Where(x => x.ItemPanel.IsVisible && x.ItemPanel.IsEnabled)
				.ToArray();

			IsCursorInteractionEnabled = (activePanels.Length > 0);

			ActiveCursorTypes = vInteractSett.Cursors;

			vActiveCursorInteractions = vAllItems
				.Select(x => x.Item)
				.Cast<IBaseItemInteractionState>()
				.ToArray();

			vActiveCursorPlanes = activePanels
				.Select(x => x.InteractionPlane)
				.ToArray();

			foreach ( ProjectionState proj in vProjectionMap.Values ) {
				UpdateProjection(proj);
			}

			foreach ( ItemTree itemTree in vAllItems ) {
				itemTree.Item.UpdateSelectionProcess();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateProjection(ProjectionState pProj) {
			ICursorState cursor = pProj.Cursor;
			CursorType cursorType = cursor.Type;
			bool allowSelect = (cursor.IsInputAvailable && pProj.IsActive);
			Vector3? cursorWorldPos = (allowSelect ? cursor.GetWorldPosition() : (Vector3?)null);
			ItemTree nearestTree = new ItemTree();
			float nearestDist = float.MaxValue;

			foreach ( ItemTree itemTree in vAllItems ) {
				itemTree.Item.UpdateWithCursor(cursorType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				float itemDist = itemTree.Item.GetHighlightDistance(cursorType);

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


		////////////////////////////////////////////////////////////////////////////////////////////////
		// IHovercursorDelegate
		/*--------------------------------------------------------------------------------------------*/
		public CursorDomain Domain {
			get {
				return CursorDomain.Hoverboard;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsCursorInteractionEnabled { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public CursorType[] ActiveCursorTypes { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public float CursorDisplayStrength {
			get {
				return 1;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemInteractionState[] GetActiveCursorInteractions(CursorType pCursorType) {
			return vActiveCursorInteractions;
		}

		/*--------------------------------------------------------------------------------------------*/
		public PlaneData[] GetActiveCursorPlanes(CursorType pCursorType) {
			return vActiveCursorPlanes;
		}

	}

}
