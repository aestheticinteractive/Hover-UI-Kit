using System.Collections.Generic;
using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Navigation;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class HoverboardState : IHoverboardState {

		private struct ButtonTree {
			public PanelState Panel;
			public GridState Grid;
			public BaseItemState Button;
		}

		public HoverboardCustomizationProvider CustomizationProvider { get; private set; }
		public HoverboardPanelProvider[] PanelProviders { get; private set; }
		public IHovercursorState HovercursorState { get; private set; }

		public PanelState[] Panels { get; private set; }

		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, ProjectionState> vProjectionMap;
		private readonly ButtonTree[] vAllButtons;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverboardState(HoverboardCustomizationProvider pCustom, 
												HoverboardPanelProvider[] pPanels, Transform pBaseTx) {
			CustomizationProvider = pCustom;
			PanelProviders = pPanels;
			
			vBaseTx = pBaseTx;
			vProjectionMap = new Dictionary<CursorType, ProjectionState>();

			////

			IEnumerable<ItemPanel> itemPanels = PanelProviders.Select(x => x.GetPanel());
			InteractionSettings interSett = CustomizationProvider.GetInteractionSettings();
			var panels = new List<PanelState>();
			var allButtons = new List<ButtonTree>();

			foreach ( ItemPanel itemPanel in itemPanels ) {
				var panel = new PanelState(itemPanel, interSett);
				panels.Add(panel);

				foreach ( GridState grid in panel.Grids ) {
					foreach ( BaseItemState button in grid.Items ) {
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
		public void SetHovercursorState(IHovercursorState pHovercursorState) {
			HovercursorState = pHovercursorState;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public ProjectionState GetProjectionState(CursorType pCursorType) {
			if ( !vProjectionMap.ContainsKey(pCursorType) ) {
				ICursorState cursorState = HovercursorState.GetCursorState(pCursorType);

				var projState = new ProjectionState(cursorState,
					CustomizationProvider.GetInteractionSettings(), vBaseTx);

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
			ButtonTree nearestTree = new ButtonTree();
			float nearestDist = float.MaxValue;

			foreach ( ButtonTree buttonTree in vAllButtons ) {
				BaseItemState button = buttonTree.Button;
				button.UpdateWithCursor(cursorType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				float buttonDist = button.GetHighlightDistance(cursorType);

				if ( buttonDist >= nearestDist ) {
					continue;
				}

				nearestTree = buttonTree;
				nearestDist = buttonDist;
			}

			foreach ( ButtonTree buttonTree in vAllButtons ) {
				BaseItemState button = buttonTree.Button;
				button.SetAsNearestItem(cursorType, (button == nearestTree.Button));
			}

			if ( nearestTree.Panel == null || nearestTree.Button.MaxHighlightProgress <= 0 ) {
				pProjection.SetNearestPanelTransform(null);
				pProjection.NearestButtonHighlightProgress = 0;
				return;
			}

			GameObject panelObj = (GameObject)nearestTree.Panel.ItemPanel.DisplayContainer;
			pProjection.SetNearestPanelTransform(panelObj.transform);
			pProjection.NearestButtonHighlightProgress = 
				nearestTree.Button.GetHighlightProgress(cursorType);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAfterAllCursors() {
			foreach ( ButtonTree buttonTree in vAllButtons ) {
				buttonTree.Button.UpdateSelectionProcess();
			}
		}

	}

}
