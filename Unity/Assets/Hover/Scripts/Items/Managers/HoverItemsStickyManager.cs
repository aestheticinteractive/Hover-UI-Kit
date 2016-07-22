using System;
using System.Collections.Generic;
using Hover.Cursors;
using Hover.Renderers;
using UnityEngine;

namespace Hover.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsStickyManager : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		private List<HoverItemData> vItemDatas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vItemDatas = new List<HoverItemData>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			GetComponent<HoverItemsManager>().FillListWithExistingItemComponents(vItemDatas);
			ClearCursorLists();
			FillCursorLists();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ClearCursorLists() {
			List<IHoverCursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;

			for ( int ci = 0 ; ci < cursorCount ; ci++ ) {
				cursors[ci].ActiveStickySelections.Clear();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FillCursorLists() {
			for ( int i = 0 ; i < vItemDatas.Count ; i++ ) {
				HoverItemData data = vItemDatas[i];
				ISelectableItemData selData = (data as ISelectableItemData);

				if ( selData == null || !selData.IsStickySelected ) {
					continue;
				}

				HoverItemHighlightState highState = data.GetComponent<HoverItemHighlightState>();

				if ( highState.NearestHighlight == null ) {
					continue;
				}

				IHoverCursorData cursorData = highState.NearestHighlight.Value.Cursor;
				HoverItemSelectionState selState = data.GetComponent<HoverItemSelectionState>();
				HoverRenderer rend = data.GetComponent<HoverRendererUpdater>().ActiveRenderer;

				var info = new StickySelectionInfo {
					ItemWorldPosition = rend.GetCenterWorldPosition(),
					SelectionProgress = selState.SelectionProgress
				};

				cursorData.ActiveStickySelections.Add(info);
			}
		}

	}

}
