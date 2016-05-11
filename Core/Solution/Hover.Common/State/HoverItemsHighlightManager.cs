using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hover.Common.Input;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class HoverItemsHighlightManager : MonoBehaviour {

		public HovercursorDataProvider CursorDataProvider;

		private List<HoverItemHighlightState> vHighStates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}
			
			vHighStates = new List<HoverItemHighlightState>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vHighStates = FindObjectsOfType<HoverItemHighlightState>().ToList();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ResetItems();
			
			List<HovercursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				HovercursorData cursor = cursors[i];
				HoverItemHighlightState item = FindNearestItemToCursor(cursor.Type);
				
				if ( item == null ) {
					continue;
				}
				
				item.SetNearestAcrossAllItemsForCursor(cursor.Type);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(HoverItemHighlightState pHighState) {
			if ( vHighStates.Contains(pHighState) ) {
				Debug.LogWarning("Cannot add duplicate item '"+pHighState.name+"'.", pHighState);
				return;
			}
		
			vHighStates.Add(pHighState);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void RemoveItem(HoverItemHighlightState pHighState) {
			if ( !vHighStates.Remove(pHighState) ) {
				Debug.LogWarning("Cannot remove missing item '"+pHighState.name+"'.", pHighState);
				return;
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ResetItems() {
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vHighStates[i];
				
				if ( highState == null ) {
					vHighStates.RemoveAt(i);
					i--;
					Debug.LogWarning("Found and removed a null item; use RemoveItem() instead.");
					continue;
				}
				
				highState.ResetAllNearestStates();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemHighlightState FindNearestItemToCursor(CursorType pCursorType) {
			float maxProg = 0;
			HoverItemHighlightState nearestItem = null;
			
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState item = vHighStates[i];
				
				if ( !item.gameObject.activeInHierarchy || item.IsHighlightPrevented ) {
					continue;
				}
				
				HoverItemHighlightState.Highlight? high = item.GetHighlight(pCursorType);
				
				if ( high == null || high.Value.Progress <= maxProg ) {
					continue;
				}
				
				maxProg = high.Value.Progress;
				nearestItem = item;
			}
			
			return nearestItem;
		}

	}

}
