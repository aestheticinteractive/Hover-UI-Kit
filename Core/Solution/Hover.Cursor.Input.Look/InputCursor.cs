using System;
using Hover.Common.Input;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Input.Look {

	/*================================================================================================*/
	public class InputCursor : IInputCursor {

		public CursorType Type { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }

		private readonly InputSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputCursor(CursorType pType, InputSettings pSettings) {
			Type = pType;
			vSettings = pSettings;
			Rotation = Quaternion.identity;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithPlanes(InteractionPlaneState[] pPlanes) {
			IsAvailable = false;
			Position = Vector3.zero;
			Size = 0;

			if ( pPlanes.Length == 0 ) {
				return;
			}

			////

			float minDist = float.MaxValue;
			InteractionPlaneState nearest = null;

			foreach ( InteractionPlaneState plane in pPlanes ) {
				if ( !plane.IsEnabled ) {
					continue;
				}

				UpdateWithPlane(plane);

				if ( plane.IsHit && plane.HitDist < minDist ) {
					minDist = plane.HitDist;
					nearest = plane;
				}
			}

			if ( nearest == null ) {
				return;
			}

			////

			nearest.IsNearest = true;

			IsAvailable = true;
			Position = nearest.HitPosition;
			Size = vSettings.CursorSize;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithPlane(InteractionPlaneState pPlane) {
			Transform localTx = vSettings.InputTransform;
			Transform camTx = vSettings.CameraTransform;

			Vector3 camPosLocal = localTx.InverseTransformPoint(camTx.position);
			Vector3 camDirLocal = localTx.InverseTransformDirection(camTx.rotation*Vector3.forward);
			Vector3 planePosLocal = localTx.InverseTransformPoint(pPlane.PointWorld);
			Vector3 planeNormLocal = localTx.InverseTransformDirection(pPlane.NormalWorld);

			float numer = Vector3.Dot(planePosLocal-camPosLocal, planeNormLocal);
			float denom = Vector3.Dot(camDirLocal, planeNormLocal);

			if ( denom == 0 ) { //exactly parallel (very unlikely scenario)
				return;
			}

			float t = numer/denom;

			pPlane.IsHit = true;
			pPlane.HitDist = Math.Abs(t);
			pPlane.HitPosition = camPosLocal+camDirLocal*t;
		}

	}

}
