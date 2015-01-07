using System;
using HandMenu.Input;
using UnityEngine;

namespace HandMenu.State {

	/*================================================================================================*/
	public class MenuPointState {

		public PointData.PointZone Zone { get; set; }
		public bool IsActive { get; private set; }
		public Vector3 Position { get; private set; }
		public Vector3 Direction { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Strength { get; private set; }
		public Vector3 SelectionPosition { get; private set; }
		public float SelectionDistance { get; private set; }
		public float SelectionProgress { get; private set; }

		private readonly PointProvider vPointProv;
		private float vSelectionExtension;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuPointState(PointData.PointZone pZone, PointProvider pPointProv) {
			Zone = pZone;
			vPointProv = pPointProv;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			PointData data = vPointProv.Data;

			if ( data == null ) {
				IsActive = false;
				Position = Vector3.zero;
				Direction = Vector3.zero;
				Rotation = Quaternion.identity;
				Strength = 0;
				SelectionPosition = Vector3.zero;
				SelectionDistance = 0;
				SelectionProgress = 0;
			}
			else {
				IsActive = true;
				Position = data.Position;
				Direction = data.Direction;
				Rotation = data.Rotation;
				Strength = data.Extension;
				CalcSelectionPosition();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(Vector3? pCursorPosition) {
			if ( pCursorPosition == null || !IsActive ) {
				SelectionProgress = 0;
				return;
			}

			SelectionDistance = (SelectionPosition-(Vector3)pCursorPosition).magnitude;

			float prog = (0.16f-SelectionDistance)/0.16f;
			SelectionProgress = Math.Max(0, Math.Min(1, prog*1.2f));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetSelectionExtension(float pExtension) {
			vSelectionExtension = pExtension;
			CalcSelectionPosition();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CalcSelectionPosition() {
			SelectionPosition = Position - Direction*vSelectionExtension;
		}

	}

}
