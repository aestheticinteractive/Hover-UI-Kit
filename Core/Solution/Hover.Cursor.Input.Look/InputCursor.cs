using System;
using Hover.Common.Input;
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

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithPlanes(InputPlane[] pPlanes) {
			if ( pPlanes.Length == 0 ) {
				IsAvailable = false;
				Position = Vector3.zero;
				return;
			}

			////

			float minDist = float.MaxValue;
			InputPlane nearest = null;

			foreach ( InputPlane plane in pPlanes ) {
				UpdateWithPlane(plane);

				if ( plane.IsHit && plane.HitDist < minDist ) {
					minDist = plane.HitDist;
					nearest = plane;
				}
			}

			////

			if ( nearest == null ) {
				IsAvailable = false;
				Position = Vector3.zero;
				Size = 0;
				return;
			}

			nearest.IsNearest = true;

			IsAvailable = true;
			Position = nearest.HitPosition;
			Size = vSettings.CursorSize;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithPlane(InputPlane pPlane) {
			//TODO: "planePos" is shifted forward slighty to negate the effect of UiLevel.PushFromHand.
			//This isn't exact. Could probably improve this by moving this "push" effect into the Leap
			//input module. That way, the menu would appear exactly where the input data specifies. It
			//becomes the input module's responsibility to offset the menu position to provide enough
			//space for things like 3D hand models.

			Transform localTx = vSettings.InputTransform;
			Transform camTx = vSettings.CameraTransform;

			Vector3 camPosLocal = localTx.InverseTransformPoint(camTx.position);
			Vector3 camDirLocal = localTx.InverseTransformDirection(camTx.rotation*Vector3.forward);
			//Vector3 camHorizLocal = localTx.InverseTransformDirection(camTx.rotation*Vector3.left);
			Vector3 planePosLocal = localTx.InverseTransformPoint(pPlane.PointWorld);
			Vector3 planeNormLocal = localTx.InverseTransformDirection(pPlane.NormalWorld);

			//camPosLocal += camHorizLocal*vSettings.CursorHorizontalOffset*0.1f;

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
