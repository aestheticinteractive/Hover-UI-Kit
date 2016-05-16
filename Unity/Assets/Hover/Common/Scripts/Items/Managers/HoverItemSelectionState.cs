using System;
using Hover.Common.Input;
using UnityEngine;

namespace Hover.Common.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemData))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverItemSelectionState : MonoBehaviour {

		public bool IsSelectionPrevented { get; private set; }
		
		private readonly BaseInteractionSettings vSettings;
		
		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemSelectionState() {
			vSettings = new BaseInteractionSettings(); //TODO: access from somewhere
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					BaseItem itemData = GetComponent<HoverItemData>().Data;
					ISelectableItem selData = (itemData as ISelectableItem);

					if ( selData == null || !selData.IsStickySelected ) {
						return 0;
					}
					
					HoverItemHighlightState.Highlight? nearestHigh = 
						GetComponent<HoverItemHighlightState>().NearestHighlight;
					float minHighDist = (nearestHigh == null ? 
						float.MaxValue : nearestHigh.Value.Distance);

					return Mathf.InverseLerp(vSettings.StickyReleaseDistance/vSettings.ScaleMultiplier,
						vDistanceUponSelection, minHighDist);
				}
				
				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/vSettings.SelectionMilliseconds);
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
			
			BaseItem itemData = GetComponent<HoverItemData>().Data;
			ISelectableItem selData = (itemData as ISelectableItem);
			
			vSelectionStart = null;
			
			if ( selData != null ) {
				selData.DeselectStickySelections();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private bool UpdateSelectionProgress() {
			BaseItem itemData = GetComponent<HoverItemData>().Data;
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
