using System;
using System.Collections.Generic;
using Hover.Common.Input;
using Hover.Common.Renderers;
using UnityEngine;

namespace Hover.Common.Items.Managers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemHighlightState : MonoBehaviour {

		[Serializable]
		public struct Highlight {
			public bool IsNearestAcrossAllItems;
			public IHoverCursorData Cursor;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public bool IsHighlightPrevented { get; private set; }
		public Highlight? NearestHighlight { get; private set; }
		public List<Highlight> Highlights { get; private set; }
		public bool IsNearestAcrossAllItemsForAnyCursor { get; private set; }
		
		public HoverCursorDataProvider CursorDataProvider;
		public HoverRendererController ProximityProvider;
		public HoverInteractionSettings InteractionSettings;

		private readonly HashSet<string> vPreventHighlightMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemHighlightState() {
			Highlights = new List<Highlight>();
			vPreventHighlightMap = new HashSet<string>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}

			if ( ProximityProvider == null ) {
				ProximityProvider = GetComponent<HoverRendererController>();
			}

			if ( InteractionSettings == null ) {
				InteractionSettings = (GetComponent<HoverInteractionSettings>() ?? 
					FindObjectOfType<HoverInteractionSettings>());
			}

			if ( CursorDataProvider == null ) {
				Debug.LogWarning("Could not find 'CursorDataProvider'.");
			}

			if ( ProximityProvider == null ) {
				Debug.LogWarning("Could not find 'ProximityProvider'.");
			}

			if ( InteractionSettings == null ) {
				Debug.LogWarning("Could not find 'InteractionSettings'.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Highlights.Clear();
			NearestHighlight = null;
			
			UpdateIsHighlightPrevented();

			if ( IsHighlightPrevented || ProximityProvider == null || 
					CursorDataProvider == null || InteractionSettings == null ) {
				return;
			}

			AddLatestHighlightsAndFindNearest();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Highlight? GetHighlight(CursorType pType) {
			for ( int i = 0 ; i < Highlights.Count ; i++ ) {
				Highlight high = Highlights[i];
				
				if ( high.Cursor.Type == pType ) {
					return high;
				}
			}

			return null;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float MaxHighlightProgress {
			get {
				//TODO: how did this (highlight during/after a sticky selection) used to work?
				/*BaseItem itemData = GetComponent<HoverItemData>().Data;
				ISelectableItem selData = (itemData as ISelectableItem);
				
				if ( selData != null && selData.IsStickySelected ) {
					return 1;
				}*/
				
				Highlight? nearestHigh = GetComponent<HoverItemHighlightState>().NearestHighlight;
				return (nearestHigh == null ? 0 : nearestHigh.Value.Progress);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void ResetAllNearestStates() {
			for ( int i = 0 ; i < Highlights.Count ; i++ ) {
				Highlight high = Highlights[i];
				high.IsNearestAcrossAllItems = false;
				Highlights[i] = high;
			}
			
			IsNearestAcrossAllItemsForAnyCursor = false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetNearestAcrossAllItemsForCursor(CursorType pType) {
			int highForCursorI = -1;
		
			for ( int i = 0 ; i < Highlights.Count ; i++ ) {
				Highlight high = Highlights[i];
				
				if ( high.Cursor.Type == pType ) {
					highForCursorI = i;
					break;
				}
			}
			
			if ( highForCursorI == -1 ) {
				throw new Exception("No highlight found for type '"+pType+"'.");
			}
			
			Highlight highForCursor = Highlights[highForCursorI];
			highForCursor.IsNearestAcrossAllItems = true;
			Highlights[highForCursorI] = highForCursor;
			
			IsNearestAcrossAllItemsForAnyCursor = true;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PreventHighlightViaDisplay(string pName, bool pPrevent) {
			if ( pPrevent ) {
				vPreventHighlightMap.Add(pName);
			}
			else {
				vPreventHighlightMap.Remove(pName);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsHighlightPreventedViaAnyDisplay() {
			return (vPreventHighlightMap.Count > 0);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsHighlightPreventedViaDisplay(string pName) {
			return vPreventHighlightMap.Contains(pName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIsHighlightPrevented() {
			HoverItem hoverItem = GetComponent<HoverItem>();
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			ISelectableItem selData = (itemData as ISelectableItem);
			
			IsHighlightPrevented = (
				selData == null ||
				!itemData.IsEnabled ||
				//!itemData.IsVisible ||
				!itemData.IsAncestryEnabled ||
				//!itemData.IsAncestryVisible ||
				!hoverItem.gameObject.activeInHierarchy ||
				IsHighlightPreventedViaAnyDisplay()
			);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void AddLatestHighlightsAndFindNearest() {
			float minDist = float.MaxValue;
			List<IHoverCursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				IHoverCursorData cursor = cursors[i];

				if ( !cursor.IsActive ) {
					continue;
				}

				Highlight high = CalculateHighlight(cursor);
				Highlights.Add(high);

				if ( high.Distance >= minDist ) {
					continue;
				}

				minDist = high.Distance;
				NearestHighlight = high;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private Highlight CalculateHighlight(IHoverCursorData pCursor) {
			var high = new Highlight();
			high.Cursor = pCursor;
			
			if ( !Application.isPlaying ) {
				return high;
			}
			
			high.NearestWorldPos = ProximityProvider.GetNearestWorldPosition(pCursor.WorldPosition);
			high.Distance = (pCursor.WorldPosition-high.NearestWorldPos).magnitude;
			high.Progress = Mathf.InverseLerp(InteractionSettings.HighlightDistanceMax,
				InteractionSettings.HighlightDistanceMin, high.Distance);
			
			return high;
		}

	}

}
