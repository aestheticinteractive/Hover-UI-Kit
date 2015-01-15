using System;
using Henu.Navigation;
using Henu.Settings;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class ArcSegmentState {

		public NavItem NavItem { get; private set; }

		public float HighlightDistance { get; private set; }
		public float HighlightProgress { get; private set; }

		private readonly InteractionSettings vSettings;
		private readonly float vHighlightDistRange;
		private Func<Vector3, float> vCursorDistanceFunc;
		private DateTime? vSelectionStart;
		private bool vIsAnimating;
		private bool vPreventSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcSegmentState(NavItem pNavItem, InteractionSettings pSettings) {
			NavItem = pNavItem;
			vSettings = pSettings;
			vHighlightDistRange = vSettings.HighlightDistanceMax-vSettings.HighlightDistanceMin;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					return (NavItem.IsStickySelected() ? HighlightProgress : 0);
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
				vPreventSelection = false;
				return;
			}

			if ( vCursorDistanceFunc == null ) {
				throw new Exception("No CursorDistanceFunction has been set.");
			}

			float dist = vCursorDistanceFunc((Vector3)pCursorPosition);
			float prog = 1-(dist-vSettings.HighlightDistanceMin)/vHighlightDistRange;

			HighlightDistance = dist;
			HighlightProgress = Math.Max(0, Math.Min(1, prog));
		}

		/*--------------------------------------------------------------------------------------------*/
		internal bool SetAsNearestSegment(bool pIsNearest) {
			if ( NavItem.IsStickySelected() ) {
				if ( !pIsNearest || HighlightProgress <= 0 ) {
					NavItem.Deselect();
				}
			}

			if ( !pIsNearest || HighlightProgress < 1 ) {
				vSelectionStart = null;
				vPreventSelection = false;
				return false;
			}

			if ( vPreventSelection ) {
				vSelectionStart = null;
				return false;
			}

			if ( NavItem.Selected && NavItem.Type == NavItem.ItemType.Radio ) {
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

			NavItem.Select();
			vSelectionStart = null;
			vPreventSelection = true;
			return true;
		}

	}

}
