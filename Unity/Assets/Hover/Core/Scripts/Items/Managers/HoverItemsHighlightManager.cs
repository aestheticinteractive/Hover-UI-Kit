using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsHighlightManager : MonoBehaviour {

		[Serializable]
		public class ItemSelectedEvent : UnityEvent<HoverItem, ICursorData> {}

		public HoverCursorDataProvider CursorDataProvider;
		public ItemSelectedEvent OnItemSelected;

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

				HoverItemHighlightState highState = FindNearestItemWithinProxOfCursor(
					cursor.Type, out HoverItemHighlightState.Highlight? high);

				if ( highState == null || high == null ) {
					continue;
				}

				highState.SetNearestAcrossAllItemsForCursor(cursor.Type);
				cursor.MaxItemHighlightProgress = high.Value.Progress;
			}

			for ( int i = 0 ; i < vActiveHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vActiveHighStates[i];
				HoverItemSelectionState selState = highState.GetComponent<HoverItemSelectionState>();

				highState.UpdateViaManager();

				if ( selState == null ) {
					continue;
				}

				selState.UpdateViaManager();

				if ( selState.WasSelectedThisFrame ) {
					ICursorData selCursor = highState.NearestHighlight?.Cursor;
					//Debug.Log("Item selected: "+selState.transform.ToDebugPath()+" / "+
					//	selCursor?.Type, selState);
					OnItemSelected.Invoke(highState.GetComponent<HoverItem>(), selCursor);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverItemHighlightState FindNearestItemWithinProxOfCursor(CursorType pCursorType, 
												out HoverItemHighlightState.Highlight? pNearestHigh) {
			float minDist = float.MaxValue;
			HoverItemHighlightState nearestItem = null;

			pNearestHigh = null;

			for ( int i = 0 ; i < vActiveHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vActiveHighStates[i];
				HoverItemHighlightState.Highlight? high = highState.GetHighlight(pCursorType);

				if ( high == null || high.Value.Progress <= 0 || high.Value.Distance >= minDist ) {
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
