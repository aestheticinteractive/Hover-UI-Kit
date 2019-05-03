using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsHighlightManager : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;

		private List<HoverItemHighlightState> vActiveHighStates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = HoverCursorDataProvider.Instance;
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vActiveHighStates = new List<HoverItemHighlightState>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();

			itemsMan.FillListWithActiveItemComponents(vActiveHighStates);
			ResetItems();
			UpdateItems();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ResetItems() {
			for ( int i = 0 ; i < vActiveHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vActiveHighStates[i];

				if ( highState == null ) {
					vActiveHighStates.RemoveAt(i);
					i--;
					Debug.LogError("Found and removed a null item; use RemoveItem() instead.");
					continue;
				}
	
				highState.ResetAllNearestStates();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItems() {
			List<ICursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				ICursorData cursor = cursors[i];
				cursor.MaxItemHighlightProgress = 0;
				cursor.MaxItemSelectionProgress = 0;

				if ( !cursor.CanCauseSelections ) {
					continue;
				}

				HoverItemHighlightState.Highlight? high;
				HoverItemHighlightState highState = FindNearestItemToCursor(cursor.Type, out high);

				if ( highState == null || high == null || high.Value.Progress <= 0 ) {
					continue;
				}

				highState.SetNearestAcrossAllItemsForCursor(cursor.Type);
				cursor.MaxItemHighlightProgress = high.Value.Progress;
			}

			for ( int i = 0 ; i < vActiveHighStates.Count ; i++ ) {
				vActiveHighStates[i].UpdateHighlightState();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverItemHighlightState FindNearestItemToCursor(CursorType pCursorType, 
												out HoverItemHighlightState.Highlight? pNearestHigh) {
			float minDist = float.MaxValue;
			HoverItemHighlightState nearestItem = null;

			pNearestHigh = null;

			for ( int i = 0 ; i < vActiveHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vActiveHighStates[i];
				HoverItemHighlightState.Highlight? high = highState.GetHighlight(pCursorType);

				if ( high == null || high.Value.Distance >= minDist ) {
					continue;
				}

				minDist = high.Value.Distance;
				nearestItem = highState;
				pNearestHigh = high;
			}

			return nearestItem;
		}

	}

}
