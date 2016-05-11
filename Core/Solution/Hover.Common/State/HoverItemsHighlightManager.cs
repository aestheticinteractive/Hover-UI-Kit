using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hover.Common.Input;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class HoverItemsHighlightManager : MonoBehaviour {

		public HovercursorDataProvider CursorDataProvider;

		private List<HoverItemCursorActivity> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}
			
			vItems = new List<HoverItemCursorActivity>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItems = FindObjectsOfType<HoverItemCursorActivity>().ToList();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ResetItems();
			
			List<HovercursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				HovercursorData cursor = cursors[i];
				HoverItemCursorActivity item = FindNearestItemToCursor(cursor.Type);
				
				if ( item == null ) {
					continue;
				}
				
				item.SetNearestAcrossAllItemsForCursor(cursor.Type);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(HoverItemCursorActivity pItem) {
			if ( vItems.Contains(pItem) ) {
				Debug.LogWarning("Cannot add duplicate item '"+pItem.name+"'.", pItem);
				return;
			}
		
			vItems.Add(pItem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void RemoveItem(HoverItemCursorActivity pItem) {
			if ( !vItems.Remove(pItem) ) {
				Debug.LogWarning("Cannot remove missing item '"+pItem.name+"'.", pItem);
				return;
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ResetItems() {
			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItemCursorActivity hoverItemCursorAct = vItems[i];
				
				if ( hoverItemCursorAct == null ) {
					vItems.RemoveAt(i);
					i--;
					Debug.LogWarning("Found and removed a null item; use RemoveItem() instead.");
					continue;
				}
				
				hoverItemCursorAct.ResetAllNearestStates();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemCursorActivity FindNearestItemToCursor(CursorType pCursorType) {
			float maxProg = 0;
			HoverItemCursorActivity nearestItem = null;
			
			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItemCursorActivity item = vItems[i];
				
				if ( !item.gameObject.activeInHierarchy || item.IsHighlightPrevented ) {
					continue;
				}
				
				HoverItemCursorActivity.Highlight? high = item.GetHighlight(pCursorType);
				
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
