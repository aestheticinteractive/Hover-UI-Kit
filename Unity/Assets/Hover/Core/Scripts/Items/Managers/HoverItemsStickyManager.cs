using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using Hover.Core.Renderers;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsStickyManager : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		private List<HoverItem> vItems;
		private Func<HoverItem, bool> vItemFilterFunc;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = HoverCursorDataProvider.Instance;
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vItems = new List<HoverItem>();
			vItemFilterFunc = FilterItems;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			GetComponent<HoverItemsManager>().FillListWithMatchingItems(vItems, true, vItemFilterFunc);
			ClearCursorLists();
			FillCursorLists();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool FilterItems(HoverItem pItem) {
			IItemDataSelectable selData = (pItem.Data as IItemDataSelectable);

			if ( selData == null || !selData.IsStickySelected || !selData.AllowIdleDeselection ) {
				return false;
			}

			return (pItem.GetComponent<HoverItemHighlightState>().NearestHighlight != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void ClearCursorLists() {
			List<ICursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;

			for ( int ci = 0 ; ci < cursorCount ; ci++ ) {
				cursors[ci].ActiveStickySelections.Clear();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FillCursorLists() {
			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItemData data = vItems[i].Data;
				IItemDataSelectable selData = (IItemDataSelectable)data;
				HoverItemHighlightState highState = data.GetComponent<HoverItemHighlightState>();
				ICursorData cursorData = highState.NearestHighlight.Value.Cursor;

				if ( cursorData.Idle.Progress >= 1 ) {
					selData.DeselectStickySelections();
					continue;
				}

				HoverItemSelectionState selState = data.GetComponent<HoverItemSelectionState>();
				HoverRenderer rend = data.GetComponent<HoverItemRendererUpdater>().ActiveRenderer;

				var info = new StickySelectionInfo {
					ItemWorldPosition = rend.GetCenterWorldPosition(),
					SelectionProgress = selState.SelectionProgress
				};

				cursorData.ActiveStickySelections.Add(info);
			}
		}

	}

}
