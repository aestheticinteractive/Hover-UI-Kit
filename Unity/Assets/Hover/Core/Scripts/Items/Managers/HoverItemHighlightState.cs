using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemHighlightState : MonoBehaviour {

		[Serializable]
		public struct Highlight {
			public bool IsNearestAcrossAllItems;
			public ICursorData Cursor;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public bool IsHighlightPrevented { get; private set; }
		public Highlight? NearestHighlight { get; private set; }
		public List<Highlight> Highlights { get; private set; }
		public bool IsNearestAcrossAllItemsForAnyCursor { get; private set; }
		
		public HoverCursorDataProvider CursorDataProvider;
		public HoverItemRendererUpdater ProximityProvider;
		public HoverInteractionSettings InteractionSettings;

		private readonly HashSet<string> vPreventHighlightMap;
		private readonly HashSet<CursorType> vIsNearestForCursorTypeMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemHighlightState() {
			Highlights = new List<Highlight>();
			vPreventHighlightMap = new HashSet<string>();
			vIsNearestForCursorTypeMap = new HashSet<CursorType>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}

			if ( ProximityProvider == null ) {
				ProximityProvider = GetComponent<HoverItemRendererUpdater>();
			}

			if ( InteractionSettings == null ) {
				InteractionSettings = (GetComponent<HoverInteractionSettings>() ??
					FindObjectOfType<HoverItemsManager>().GetComponent<HoverInteractionSettings>());
			}

			if ( CursorDataProvider == null ) {
				Debug.LogWarning("Could not find 'CursorDataProvider'.");
			}

			if ( ProximityProvider == null ) {
				//TODO: show warning elsewhere? the renderer is typically added *after* this
				//Debug.LogWarning("Could not find 'ProximityProvider'.");
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
				IItemDataSelectable selData = (GetComponent<HoverItemData>() as IItemDataSelectable);
				
				if ( selData != null && selData.IsStickySelected ) {
					return 1;
				}
				
				return (NearestHighlight == null ? 0 : NearestHighlight.Value.Progress);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void ResetAllNearestStates() {
			vIsNearestForCursorTypeMap.Clear();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetNearestAcrossAllItemsForCursor(CursorType pType) {
			vIsNearestForCursorTypeMap.Add(pType);
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
			IItemDataSelectable selData = (itemData as IItemDataSelectable);
			
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
			List<ICursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				ICursorData cursor = cursors[i];

				if ( !cursor.CanCauseSelections ) {
					continue;
				}

				Highlight high = CalculateHighlight(cursor);
				high.IsNearestAcrossAllItems = vIsNearestForCursorTypeMap.Contains(cursor.Type);
				Highlights.Add(high);

				if ( high.Distance >= minDist ) {
					continue;
				}

				minDist = high.Distance;
				NearestHighlight = high;
			}

			IsNearestAcrossAllItemsForAnyCursor = (vIsNearestForCursorTypeMap.Count > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		private Highlight CalculateHighlight(ICursorData pCursor) {
			var high = new Highlight();
			high.Cursor = pCursor;
			
			if ( !Application.isPlaying ) {
				return high;
			}

			Vector3 cursorWorldPos = (pCursor.BestRaycastResult == null ?
				pCursor.WorldPosition : pCursor.BestRaycastResult.Value.WorldPosition);

			high.NearestWorldPos = ProximityProvider.GetNearestWorldPosition(cursorWorldPos);
			high.Distance = (cursorWorldPos-high.NearestWorldPos).magnitude;
			high.Progress = Mathf.InverseLerp(InteractionSettings.HighlightDistanceMax,
				InteractionSettings.HighlightDistanceMin, high.Distance);
			
			return high;
		}

	}

}
