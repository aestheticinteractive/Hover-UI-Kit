using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverCursorData))]
	public class HoverCursorIdleState : MonoBehaviour, IHoverCursorIdle {

		public struct HistoryRecord {
			public DateTime Time;
			public Vector3 WorldPosition;
		}

		public float Progress { get; private set; }
		public Vector3 WorldPosition { get; private set; }

		public HoverInteractionSettings InteractionSettings;

		private readonly List<HistoryRecord> vHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverCursorIdleState() {
			vHistory = new List<HistoryRecord>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( InteractionSettings == null ) {
				InteractionSettings = (GetComponent<HoverInteractionSettings>() ??
					FindObjectOfType<HoverInteractionSettings>());
			}

			if ( InteractionSettings == null ) {
				Debug.LogWarning("Could not find 'InteractionSettings'.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HoverCursorData data = GetComponent<HoverCursorData>();

			if ( !Application.isPlaying ) {
				Progress = 0.25f;
				WorldPosition = data.WorldPosition;
				return;
			}

			if ( data.ActiveStickySelections.Count == 0 ) {
				vHistory.Clear();
				Progress = 0;
				WorldPosition = data.WorldPosition;
				return;
			}

			if ( Progress >= 1 ) {
				vHistory.Clear();
				Progress = 0;
			}

			AddToHistory(data.WorldPosition);
			CullHistory();
			UpdateProgress();

			WorldPosition = vHistory[0].WorldPosition;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddToHistory(Vector3 pWorldPosition) {
			var current = new HistoryRecord {
				Time = DateTime.UtcNow,
				WorldPosition = pWorldPosition
			};

			vHistory.Add(current);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CullHistory() {
			float maxMs = InteractionSettings.MotionlessMilliseconds;
			float maxDistSqr = Mathf.Pow(InteractionSettings.MotionlessDistanceThreshold, 2);
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
		private void UpdateProgress() {
			HistoryRecord current = vHistory[vHistory.Count-1];
			float earliestMsAgo = (float)(current.Time-vHistory[0].Time).TotalMilliseconds;

			Progress = Mathf.Min(1, earliestMsAgo/InteractionSettings.MotionlessMilliseconds);
		}

	}

}
