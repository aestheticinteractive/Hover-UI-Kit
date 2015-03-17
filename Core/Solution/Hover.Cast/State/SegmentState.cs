using System;
using Hover.Cast.Custom;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class SegmentState : IHovercastItemState {

		public IBaseItem Item { get; private set; }

		public float HighlightDistance { get; private set; }
		public float HighlightProgress { get; private set; }
		public bool IsNearestHighlight { get; private set; }
		public bool IsSelectionPrevented { get; private set; }

		private readonly InteractionSettings vSettings;
		private Func<Vector3, float> vCursorDistanceFunc;
		private DateTime? vSelectionStart;
		private bool vIsAnimating;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SegmentState(IBaseItem pItem, InteractionSettings pSettings) {
			Item = pItem;
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					ISelectableItem selItem = (Item as ISelectableItem);

					if ( selItem == null || !selItem.IsStickySelected ) {
						return 0;
					}

					return Mathf.InverseLerp(vSettings.StickyReleaseDistance,
						vDistanceUponSelection, HighlightDistance);
				}

				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/vSettings.SelectionMilliseconds);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetCursorDistanceFunction(Func<Vector3, float> pFunc) {
			vCursorDistanceFunc = pFunc;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetIsAnimating(bool pIsAnimating) {
			vIsAnimating = pIsAnimating;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursor(Vector3? pCursorPosition) {
			if ( pCursorPosition == null || vIsAnimating ) {
				HighlightProgress = 0;
				vSelectionStart = null;
				IsSelectionPrevented = false;
				return;
			}

			if ( vCursorDistanceFunc == null ) {
				throw new Exception("No CursorDistanceFunction has been set.");
			}

			if ( !Item.IsEnabled ) {
				HighlightDistance = float.MaxValue;
				HighlightProgress = 0;
				return;
			}

			float dist = vCursorDistanceFunc((Vector3)pCursorPosition);
			float prog = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, dist);

			HighlightDistance = dist;
			HighlightProgress = prog;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal bool SetAsNearestSegment(bool pIsNearest) {
			ISelectableItem selItem = (Item as ISelectableItem);

			if ( selItem == null ) {
				return false;
			}

			IsNearestHighlight = pIsNearest;

			if ( !pIsNearest || SelectionProgress <= 0 ) {
				selItem.DeselectStickySelections();
			}

			if ( !pIsNearest || HighlightProgress < 1 ) {
				vSelectionStart = null;
				IsSelectionPrevented = false;
				return false;
			}

			if ( IsSelectionPrevented || !selItem.AllowSelection ) {
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
			vDistanceUponSelection = HighlightDistance;
			selItem.Select();
			return true;
		}

	}

}
