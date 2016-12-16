using System;
using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverItemSelectionState : MonoBehaviour {

		public float SelectionProgress { get; private set; }
		public bool IsSelectionPrevented { get; private set; }
		public bool WasSelectedThisFrame { get; private set; }
		
		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			TryResetSelection();
			UpdateSelectionProgress();
			UpdateState();
			UpdateNearestCursor();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryResetSelection() {
			if ( !GetComponent<HoverItemHighlightState>().IsHighlightPrevented ) {
				return;
			}
			
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			IItemDataSelectable selData = (itemData as IItemDataSelectable);
			
			vSelectionStart = null;
			
			if ( selData != null ) {
				selData.DeselectStickySelections();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSelectionProgress() {
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();

			if ( vSelectionStart == null ) {
				HoverItemData itemData = GetComponent<HoverItem>().Data;
				IItemDataSelectable selData = (itemData as IItemDataSelectable);

				if ( selData == null || !selData.IsStickySelected ) {
					SelectionProgress = 0;
					return;
				}
					
				HoverItemHighlightState.Highlight? nearestHigh = highState.NearestHighlight;
				float nearDist = highState.InteractionSettings.StickyReleaseDistance;
				float minHighDist = (nearestHigh == null ? float.MaxValue : nearestHigh.Value.Distance);

				SelectionProgress = Mathf.InverseLerp(nearDist, vDistanceUponSelection, minHighDist);
				return;
			}
				
			float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
			SelectionProgress = Math.Min(1, ms/highState.InteractionSettings.SelectionMilliseconds);
		}

		/*--------------------------------------------------------------------------------------------*/
		private bool UpdateState() {
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			IItemDataSelectable selData = (itemData as IItemDataSelectable);

			WasSelectedThisFrame = false;

			if ( selData == null || selData.IgnoreSelection ) {
				return false;
			}

			////

			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			bool hasNearestCursorWithFullHigh = false;

			bool canDeselect = (
				highState.IsHighlightPrevented ||
				!highState.IsNearestAcrossAllItemsForAnyCursor ||
				!selData.IsEnabled
			);

			for ( int i = 0 ; i < highState.Highlights.Count ; i++ ) {
				HoverItemHighlightState.Highlight high = highState.Highlights[i];

				if ( high.IsNearestAcrossAllItems && high.Progress >= 1 ) {
					hasNearestCursorWithFullHigh = true;
					break;
				}
			}

			if ( SelectionProgress <= 0 || canDeselect ) {
				selData.DeselectStickySelections();
			}

			if ( canDeselect || !hasNearestCursorWithFullHigh ) {
				IsSelectionPrevented = false;
				vSelectionStart = null;
				return false;
			}

			////

			if ( IsSelectionPrevented ) {
				vSelectionStart = null;
				return false;
			}

			if ( vSelectionStart == null ) {
				vSelectionStart = DateTime.UtcNow;
				return false;
			}

			if ( SelectionProgress < 1 ) {
				return false;
			}

			vSelectionStart = null;
			IsSelectionPrevented = true;
			WasSelectedThisFrame = true;
			vDistanceUponSelection = highState.NearestHighlight.Value.Distance;
			selData.Select();
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateNearestCursor() {
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemHighlightState.Highlight? nearestHigh = highState.NearestHighlight;

			if ( nearestHigh == null ) {
				return;
			}

			ICursorData cursor = nearestHigh.Value.Cursor;

			cursor.MaxItemSelectionProgress = Mathf.Max(
				cursor.MaxItemSelectionProgress, SelectionProgress);
		}

	}

}
