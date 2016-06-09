using System;
using UnityEngine;

namespace Hover.Common.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverItemSelectionState : MonoBehaviour {

		public bool IsSelectionPrevented { get; private set; }
		
		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();

				if ( vSelectionStart == null ) {
					HoverItemData itemData = GetComponent<HoverItem>().Data;
					ISelectableItem selData = (itemData as ISelectableItem);

					if ( selData == null || !selData.IsStickySelected ) {
						return 0;
					}
					
					HoverItemHighlightState.Highlight? nearestHigh = highState.NearestHighlight;
					float minHighDist = (nearestHigh == null ? 
						float.MaxValue : nearestHigh.Value.Distance);

					return Mathf.InverseLerp(highState.InteractionSettings.StickyReleaseDistance,
						vDistanceUponSelection, minHighDist);
				}
				
				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/highState.InteractionSettings.SelectionMilliseconds);
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			TryResetSelection();
			UpdateSelectionProgress();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryResetSelection() {
			if ( !GetComponent<HoverItemHighlightState>().IsHighlightPrevented ) {
				return;
			}
			
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			ISelectableItem selData = (itemData as ISelectableItem);
			
			vSelectionStart = null;
			
			if ( selData != null ) {
				selData.DeselectStickySelections();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private bool UpdateSelectionProgress() {
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			ISelectableItem selData = (itemData as ISelectableItem);

			if ( selData == null ) {
				return false;
			}

			////

			float selectProg = SelectionProgress;
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			
			bool canSelect = (
				!highState.IsHighlightPrevented && 
				highState.IsNearestAcrossAllItemsForAnyCursor &&
				selData.AllowSelection
			);
			
			if ( selectProg <= 0 || !canSelect ) {
				selData.DeselectStickySelections();
			}

			if ( !canSelect ) {
				IsSelectionPrevented = false;
				vSelectionStart = null;
				return false;
			}

			////

			HoverItemHighlightState.Highlight? nearestHigh = 
				GetComponent<HoverItemHighlightState>().NearestHighlight;
			
			if ( nearestHigh == null || nearestHigh.Value.Progress < 1 ) {
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

			if ( selectProg < 1 ) {
				return false;
			}

			vSelectionStart = null;
			IsSelectionPrevented = true;
			vDistanceUponSelection = nearestHigh.Value.Distance;
			selData.Select();
			return true;
		}

	}

}
