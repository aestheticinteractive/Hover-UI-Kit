using System;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class ArcSegmentState {

		public NavItem NavItem { get; private set; }

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
		public ArcSegmentState(NavItem pNavItem, InteractionSettings pSettings) {
			NavItem = pNavItem;
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					if ( !NavItem.IsStickySelected ) {
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

			if ( !NavItem.IsEnabled ) {
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
			IsNearestHighlight = pIsNearest;

			if ( !pIsNearest || SelectionProgress <= 0 ) {
				NavItem.DeselectStickySelections();
			}

			if ( !pIsNearest || HighlightProgress < 1 ) {
				vSelectionStart = null;
				IsSelectionPrevented = false;
				return false;
			}

			if ( IsSelectionPrevented || !NavItem.AllowSelection ) {
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
			NavItem.Select();
			return true;
		}

	}

}
