using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.State;
using Hover.Common.Util;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class HoverboardState : IHoverboardState, IHovercursorDelegate {

		private struct ItemTree {
			public PanelState Panel;
			public LayoutState Layout;
			public BaseItemState Item;
		}

		public PanelState[] FullPanels { get; private set; }
		public ReadOnlyCollection<IHoverboardPanelState> Panels { get; private set; }

		private readonly HovercursorSetup vHovercusorSetup;
		private readonly InteractionSettings vInteractSett;
		private readonly Transform vBaseTx;
		private readonly ListMap<CursorType, ProjectionState> vProjectionMap;
		private readonly ItemTree[] vAllItems;

		private readonly List<ItemTree> vActiveItems;
		private readonly ReadList<IBaseItemInteractionState> vActiveCursorInteractions;
		private readonly ReadList<PlaneData> vActiveCursorPlanes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverboardState(ItemPanel[] pItemPanels, HovercursorSetup pHovercusorSetup,
													InteractionSettings pInterSett, Transform pBaseTx) {
			vInteractSett = pInterSett;
			vHovercusorSetup = pHovercusorSetup;
			vBaseTx = pBaseTx;
			vProjectionMap = new ListMap<CursorType, ProjectionState>(EnumIntKeyComparer.CursorType);

			////

			var panels = new List<PanelState>();
			var allItems = new List<ItemTree>();

			foreach ( ItemPanel itemPanel in pItemPanels ) {
				var panel = new PanelState(itemPanel, vInteractSett);
				panel.InteractionPlane = new PlaneData("Hoverboard.Panel-"+panels.Count, 
					((GameObject)panel.ItemPanel.DisplayContainer).transform, Vector3.up);
				panels.Add(panel);

				foreach ( LayoutState layout in panel.FullLayouts ) {
					layout.ItemLayout.SetRadioSiblingsFunc(GetRadioSiblings);

					foreach ( BaseItemState item in layout.FullItems ) {
						var tree = new ItemTree {
							Panel = panel,
							Layout = layout,
							Item = item
						};

						allItems.Add(tree);
					}
				}
			}

			vAllItems = allItems.ToArray();
			vActiveItems = new List<ItemTree>();
			vActiveCursorInteractions = new ReadList<IBaseItemInteractionState>();
			vActiveCursorPlanes = new ReadList<PlaneData>();

			FullPanels = panels.ToArray();
			Panels = new ReadOnlyCollection<IHoverboardPanelState>(FullPanels);
			ActiveCursorTypes = new ReadOnlyCollection<CursorType>(vInteractSett.Cursors);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorState Hovercursor {
			get {
				return vHovercusorSetup.State;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IRadioItem> GetRadioSiblings(IRadioItem pSelectedItem) {
			var siblings = new List<IRadioItem>(); //GC_ALLOC
			string id = pSelectedItem.GroupId;

			for ( int panelI = 0 ; panelI < FullPanels.Length ; panelI++ ) {
				IHoverboardPanelState panel = FullPanels[panelI];

				for ( int layoutI = 0 ; layoutI < panel.Layouts.Count ; layoutI++ ) {
					IHoverboardLayoutState layout = panel.Layouts[layoutI];

					for ( int itemI = 0 ; itemI < layout.Items.Count ; itemI++ ) {
						IRadioItem radItem = (layout.Items[itemI].Item as IRadioItem);

						if ( radItem == null || radItem == pSelectedItem || radItem.GroupId != id ) {
							continue;
						}

						siblings.Add(radItem);
					}
				}
			}

			return siblings;
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
			IsCursorInteractionEnabled = false;

			vActiveItems.Clear();
			vActiveCursorInteractions.Clear();
			vActiveCursorPlanes.Clear();

			foreach ( PanelState panel in FullPanels ) {
				if ( !IsPanelActive(panel) ) {
					continue;
				}

				IsCursorInteractionEnabled = true;
				vActiveCursorPlanes.Add(panel.InteractionPlane);
			}

			foreach ( ItemTree itemTree in vAllItems ) {
				if ( IsItemTreeActive(itemTree) ) {
					vActiveItems.Add(itemTree);
					vActiveCursorInteractions.Add(itemTree.Item);
				}

				itemTree.Item.UpdateBeforeCursors();
			}

			for ( int i = 0 ; i < vProjectionMap.ValuesReadOnly.Count ; i++ ) {
				UpdateProjection(vProjectionMap.ValuesReadOnly[i]);
			}

			foreach ( ItemTree itemTree in vActiveItems ) {
				itemTree.Item.UpdateSelectionProcess();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool IsPanelActive(PanelState pPanel) {
			return (pPanel.ItemPanel.IsVisible && pPanel.ItemPanel.IsEnabled &&
				!pPanel.IsEveryItemSelectionPreventedViaDisplay());
		}

		/*--------------------------------------------------------------------------------------------*/
		private bool IsItemTreeActive(ItemTree pTree) {
			IBaseItem baseItem = pTree.Item.Item;
			return (baseItem.IsVisible && baseItem.IsEnabled && 
				baseItem.IsAncestryVisible && baseItem.IsAncestryEnabled);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateProjection(ProjectionState pProj) {
			ICursorState cursor = pProj.Cursor;
			CursorType cursorType = cursor.Type;
			bool allowSelect = (cursor.IsInputAvailable && pProj.IsActive);
			Vector3? cursorWorldPos = (allowSelect ? cursor.Position : (Vector3?)null);
			ItemTree nearestTree = new ItemTree();
			float nearestDist = float.MaxValue;

			foreach ( ItemTree itemTree in vActiveItems ) {
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
		public ReadOnlyCollection<CursorType> ActiveCursorTypes { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public float CursorDisplayStrength {
			get {
				return 1;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<IBaseItemInteractionState> GetActiveCursorInteractions(
																			CursorType pCursorType) {
			return vActiveCursorInteractions.ReadOnly;
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<PlaneData> GetActiveCursorPlanes(CursorType pCursorType) {
			return vActiveCursorPlanes.ReadOnly;
		}

	}

}
