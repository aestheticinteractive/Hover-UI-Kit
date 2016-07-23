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
		public float DistanceThreshold { get; private set; }

		public HoverInteractionSettings InteractionSettings;

		[Range(0, 3)]
		public float DriftStrength = 1;

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

			DistanceThreshold = InteractionSettings.MotionlessDistanceThreshold;

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
			CalcSmoothPosition();
			CullHistory();
			UpdateProgress();
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
		private void CalcSmoothPosition() {
			if ( vHistory.Count == 1 ) {
				WorldPosition = vHistory[0].WorldPosition;
				return;
			}

			HistoryRecord current = vHistory[vHistory.Count-1];
			float maxSec = 1000f/InteractionSettings.MotionlessMilliseconds;
			float smoothing = maxSec*Time.deltaTime*DriftStrength;

			WorldPosition = Vector3.Lerp(WorldPosition, current.WorldPosition, smoothing);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CullHistory() {
			HistoryRecord current = vHistory[vHistory.Count-1];
			float currDistFromCenter = (current.WorldPosition-WorldPosition).magnitude;

			if ( currDistFromCenter > InteractionSettings.MotionlessDistanceThreshold ) {
				vHistory.Clear();
				return;
			}

			float maxMs = InteractionSettings.MotionlessMilliseconds;
			int staleIndex = -1;

			for ( int i = vHistory.Count-2 ; i >= 0 ; i-- ) {
				HistoryRecord record = vHistory[i];

				if ( (current.Time-record.Time).TotalMilliseconds > maxMs ) {
					staleIndex = i;
					break;
				}

				//HistoryRecord recordPrev = vHistory[i+1];
				//Debug.DrawLine(recordPrev.WorldPosition, record.WorldPosition, Color.yellow);
			}

			if ( staleIndex < 0 ) {
				vHistory.RemoveRange(0, staleIndex+1);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateProgress() {
			if ( vHistory.Count < 2 ) {
				Progress = 0;
				return;
			}

			HistoryRecord current = vHistory[vHistory.Count-1];
			float earliestMsAgo = (float)(current.Time-vHistory[0].Time).TotalMilliseconds;

			Progress = Mathf.Min(1, earliestMsAgo/InteractionSettings.MotionlessMilliseconds);
		}

	}

}
