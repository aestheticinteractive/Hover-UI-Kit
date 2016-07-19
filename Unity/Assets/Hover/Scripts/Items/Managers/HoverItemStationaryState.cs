using System;
using System.Collections.Generic;
using Hover.Cursors;
using UnityEngine;

namespace Hover.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	[RequireComponent(typeof(HoverItemSelectionState))]
	public class HoverItemStationaryState : MonoBehaviour {

		public struct HistoryRecord {
			public HoverItemHighlightState.Highlight NearestHighlight;
			public DateTime Time;
			public Vector3 WorldPosition;
		}

		public float StationaryProgress { get; private set; }

		private readonly List<HistoryRecord> vHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemStationaryState() {
			vHistory = new List<HistoryRecord>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public HistoryRecord? GetCurrentRecord() {
			return (vHistory.Count == 0 ? (HistoryRecord?)null : vHistory[0]);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HoverItemData itemData = GetComponent<HoverItem>().Data;
			ISelectableItemData selData = (itemData as ISelectableItemData);

			if ( selData == null || !selData.IsStickySelected ) {
				vHistory.Clear();
				return;
			}

			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemHighlightState.Highlight? nearestHigh = highState.NearestHighlight;

			if ( nearestHigh == null ) {
				vHistory.Clear();
				return;
			}

			AddCursorToHistory(nearestHigh.Value);
			RemoveNonstationaryHistory(highState.InteractionSettings);
			UpdateStationaryProgress(highState.InteractionSettings);

			if ( StationaryProgress >= 1 ) {
				selData.DeselectStickySelections();
				StationaryProgress = 0;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddCursorToHistory(HoverItemHighlightState.Highlight pNearestHighlight) {
			var current = new HistoryRecord {
				NearestHighlight = pNearestHighlight,
				Time = DateTime.UtcNow,
				WorldPosition = pNearestHighlight.Cursor.WorldPosition
			};

			vHistory.Add(current);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveNonstationaryHistory(HoverInteractionSettings pInterSett) {
			float maxMs = pInterSett.MotionlessMilliseconds;
			float maxDistSqr = Mathf.Pow(pInterSett.MotionlessDistanceThreshold, 2);
			HistoryRecord current = vHistory[vHistory.Count-1];
			bool foundLast = false;

			for ( int i = vHistory.Count-2 ; i >= 0 ; i-- ) {
				HistoryRecord record = vHistory[i];

				foundLast = (
					foundLast ||
					(float)(current.Time-record.Time).TotalMilliseconds > maxMs+60 ||
					(record.WorldPosition-current.WorldPosition).sqrMagnitude > maxDistSqr
				);

				if ( foundLast ) {
					vHistory.RemoveAt(i);
					continue;
				}

				HistoryRecord recordPrev = vHistory[i+1];
				Debug.DrawLine(recordPrev.WorldPosition, record.WorldPosition, Color.yellow);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateStationaryProgress(HoverInteractionSettings pInterSett) {
			HistoryRecord current = vHistory[vHistory.Count-1];
			float earliestMsAgo = (float)(current.Time-vHistory[0].Time).TotalMilliseconds;

			StationaryProgress = Mathf.Min(1, earliestMsAgo/pInterSett.MotionlessMilliseconds);
		}

	}

}
