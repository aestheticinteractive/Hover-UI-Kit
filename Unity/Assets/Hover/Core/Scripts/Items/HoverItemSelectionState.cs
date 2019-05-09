using System;
using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items {

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
		public void UpdateViaManager() {
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			IItemDataSelectable selData = (itemData as IItemDataSelectable);

			TryResetSelection(highState, selData);
			UpdateSelectionProgress(highState, selData);
			UpdateNearestCursor(highState);
			UpdateState(selData);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ReleaseSelectionPrevention() {
			if ( IsSelectionPrevented ) {
				IsSelectionPrevented = false;
				TreeUpdater.SendTreeUpdatableChanged(this, "ReleaseSelectionPrevent");
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryResetSelection(HoverItemHighlightState pHighState,
																		IItemDataSelectable pSelData) {
			if ( !pHighState.IsHighlightPrevented ) {
				return;
			}

			vSelectionStart = null;
			pSelData?.DeselectStickySelections();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSelectionProgress(HoverItemHighlightState pHighState,
																		IItemDataSelectable pSelData) {
			if ( vSelectionStart == null ) {
				if ( pSelData == null || !pSelData.IsStickySelected ) {
					SelectionProgress = 0;
					return;
				}

				HoverItemHighlightState.Highlight? nearestHigh = pHighState.NearestHighlight;
				float nearDist = pHighState.InteractionSettings.StickyReleaseDistance;
				float minHighDist = nearestHigh?.Distance ?? float.MaxValue;

				SelectionProgress = Mathf.InverseLerp(nearDist, vDistanceUponSelection, minHighDist);
				return;
			}

			float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
			SelectionProgress = Math.Min(1, ms/pHighState.InteractionSettings.SelectionMilliseconds);
		}

		/*--------------------------------------------------------------------------------------------*/
		private bool UpdateState(IItemDataSelectable pSelData) {
			WasSelectedThisFrame = false;

			if ( pSelData == null || pSelData.IgnoreSelection ) {
				return false;
			}

			////

			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			bool hasNearestCursorWithFullHigh = false;

			bool canDeselect = (
				highState.IsHighlightPrevented ||
				!highState.IsNearestAcrossAllItemsForAnyCursor ||
				!pSelData.IsEnabled
			);

			for ( int i = 0 ; i < highState.Highlights.Count ; i++ ) {
				HoverItemHighlightState.Highlight high = highState.Highlights[i];

				if ( high.IsNearestAcrossAllItems && high.Progress >= 1 ) {
					hasNearestCursorWithFullHigh = true;
					break;
				}
			}

			if ( SelectionProgress <= 0 || canDeselect ) {
				pSelData.DeselectStickySelections();
			}

			if ( !highState.IsNearestAcrossAllItemsForAnyCursor || !hasNearestCursorWithFullHigh ) {
				IsSelectionPrevented = false;
			}

			if ( canDeselect || !hasNearestCursorWithFullHigh ) {
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
			pSelData.Select();
			return true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateNearestCursor(HoverItemHighlightState pHighState) {
			HoverItemHighlightState.Highlight? nearestHigh = pHighState.NearestHighlight;

			if ( nearestHigh == null ) {
				return;
			}

			ICursorData cursor = nearestHigh.Value.Cursor;

			cursor.MaxItemSelectionProgress = Mathf.Max(
				cursor.MaxItemSelectionProgress, SelectionProgress);
		}

	}

}
