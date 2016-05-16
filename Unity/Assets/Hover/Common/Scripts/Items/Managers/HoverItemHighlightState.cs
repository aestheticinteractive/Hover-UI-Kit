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
			public HovercursorData Data;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public bool IsHighlightPrevented { get; private set; }
		public Highlight? NearestHighlight { get; private set; }
		public List<Highlight> Highlights { get; private set; }
		public bool IsNearestAcrossAllItemsForAnyCursor { get; private set; }
		
		public HovercursorDataProvider CursorDataProvider;
		public HoverRendererController ProximityProvider;

		private readonly BaseInteractionSettings vSettings;
		private readonly HashSet<string> vPreventHighlightMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemHighlightState() {
			Highlights = new List<Highlight>();
			vSettings = new BaseInteractionSettings(); //TODO: access from somewhere
			vPreventHighlightMap = new HashSet<string>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HovercursorDataProvider>();
			}

			if ( ProximityProvider == null ) {
				ProximityProvider = GetComponent<HoverRendererController>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Highlights.Clear();
			NearestHighlight = null;
			
			UpdateIsHighlightPrevented();

			if ( !IsHighlightPrevented && ProximityProvider != null ) {
				AddLatestHighlightsAndFindNearest();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Highlight? GetHighlight(CursorType pType) {
			for ( int i = 0 ; i < Highlights.Count ; i++ ) {
				Highlight high = Highlights[i];
				
				if ( high.Data.Type == pType ) {
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
				
				if ( high.Data.Type == pType ) {
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
			HoverItemData hoverItemData = GetComponent<HoverItemData>();
			BaseItem itemData = GetComponent<HoverItemData>().Data;
			ISelectableItem selData = (itemData as ISelectableItem);
			
			IsHighlightPrevented = (
				selData == null ||
				!itemData.IsEnabled ||
				//!itemData.IsVisible ||
				!itemData.IsAncestryEnabled ||
				//!itemData.IsAncestryVisible ||
				!hoverItemData.gameObject.activeInHierarchy ||
				IsHighlightPreventedViaAnyDisplay()
			);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void AddLatestHighlightsAndFindNearest() {
			float minDist = float.MaxValue;
			
			foreach ( HovercursorData data in CursorDataProvider.Cursors ) {
				Highlight high = CalculateHighlight(data);
				Highlights.Add(high);
				
				if ( high.Distance >= minDist ) {
					continue;
				}

				minDist = high.Distance;
				NearestHighlight = high;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Highlight CalculateHighlight(HovercursorData pData) {
			var high = new Highlight();
			high.Data = pData;
			
			if ( !Application.isPlaying ) {
				return high;
			}
			
			Vector3 cursorWorldPos = pData.transform.position;
			
			high.NearestWorldPos = ProximityProvider.GetNearestWorldPosition(cursorWorldPos);
			high.Distance = (cursorWorldPos-high.NearestWorldPos).magnitude;
			high.Progress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, high.Distance*vSettings.ScaleMultiplier);
			
			return high;
		}

	}

}
