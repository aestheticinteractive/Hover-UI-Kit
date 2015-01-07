using System;
using HandMenu.Input;
using UnityEngine;

namespace HandMenu.State {

	/*================================================================================================*/
	public class MenuPointState {

		public static float HighlightDistanceMin = 0.025f;
		public static float HighlightDistanceMax = 0.12f;
		public static float SelectionMilliseconds = 500;

		public PointData.PointZone Zone { get; set; }
		public bool IsActive { get; private set; }
		public Vector3 Position { get; private set; }
		public Vector3 Direction { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Strength { get; private set; }

		public Vector3 SelectionPosition { get; private set; }
		public float HighlightDistance { get; private set; }
		public float HighlightProgress { get; private set; }

		private readonly PointProvider vPointProv;
		private float vSelectionExtension;
		private DateTime? vSelectionStart;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuPointState(PointData.PointZone pZone, PointProvider pPointProv) {
			Zone = pZone;
			vPointProv = pPointProv;
		}

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



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			PointData data = vPointProv.Data;

			if ( data == null ) {
				Reset();
			}
			else {
				UpdateWithData(data);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(Vector3? pCursorPosition) {
			if ( pCursorPosition == null || !IsActive ) {
				HighlightProgress = 0;
				return;
			}

			float dist = (SelectionPosition-(Vector3)pCursorPosition).magnitude;
			float prog = 1-(dist-HighlightDistanceMin)/(HighlightDistanceMax-HighlightDistanceMin);

			HighlightDistance = dist;
			HighlightProgress = Math.Max(0, Math.Min(1, prog));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetSelectionExtension(float pExtension) {
			vSelectionExtension = pExtension;
			CalcHighlightPosition();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ContinueSelectionProgress(bool pContinue) {
			if ( !pContinue ) {
				vSelectionStart = null;
				return;
			}

			if ( vSelectionStart == null ) {
				vSelectionStart = DateTime.UtcNow;
				return;
			}

			if ( SelectionProgress < 1 ) {
				return;
			}

			//OnSelection(this);
			vSelectionStart = null;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Reset() {
			IsActive = false;

			Position = Vector3.zero;
			Direction = Vector3.zero;
			Rotation = Quaternion.identity;
			Strength = 0;

			SelectionPosition = Vector3.zero;
			HighlightDistance = float.MaxValue;
			HighlightProgress = 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithData(PointData pData) {
			IsActive = true;

			Position = pData.Position;
			Direction = pData.Direction;
			Rotation = pData.Rotation;
			Strength = pData.Extension;

			CalcHighlightPosition();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void CalcHighlightPosition() {
			SelectionPosition = Position - Direction*vSelectionExtension;
		}

	}

}
