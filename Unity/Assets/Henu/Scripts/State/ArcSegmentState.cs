using System;
using Henu.Navigation;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class ArcSegmentState {

		public static float HighlightDistanceMin = 0.05f;
		public static float HighlightDistanceMax = 0.15f;
		public static float SelectionMilliseconds = 600;

		public NavItem NavItem { get; private set; }

		public float HighlightDistance { get; private set; }
		public float HighlightProgress { get; private set; }

		private Func<Vector3, float> vCursorDistanceFunc;
		private DateTime? vSelectionStart;
		private bool vIsAnimating;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcSegmentState(NavItem pNavItem) {
			NavItem = pNavItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					return 0;
				}

				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/SelectionMilliseconds);
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
				return;
			}

			if ( vCursorDistanceFunc == null ) {
				throw new Exception("No CursorDistanceFunction has been set.");
			}

			float dist = vCursorDistanceFunc((Vector3)pCursorPosition);
			float prog = 1-(dist-HighlightDistanceMin)/(HighlightDistanceMax-HighlightDistanceMin);

			HighlightDistance = dist;
			HighlightProgress = Math.Max(0, Math.Min(1, prog));
		}

		/*--------------------------------------------------------------------------------------------*/
		internal bool ContinueSelectionProgress(bool pContinue) {
			if ( !pContinue ) {
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
			return true;
		}

	}

}
