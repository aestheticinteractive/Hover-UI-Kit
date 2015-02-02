using System;
using Hovercast.Core.Input;
using Leap;
using UnityEngine;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	internal class LeapInputMenu : IInputMenu {

		public bool IsLeft { get; private set; }
		public bool IsActive { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Radius { get; private set; }

		public float NavigateBackStrength { get; private set; }
		public float DisplayStrength { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputMenu(bool pIsLeft) {
			IsLeft = pIsLeft;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void Rebuild(Hand pLeapHand, LeapInputSettings pSettings) {
			if ( pLeapHand == null ) {
				IsActive = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				Radius = 0;
				NavigateBackStrength = 0;
				DisplayStrength = 0;
				return;
			}

			IsActive = true;
			Position = pLeapHand.PalmPosition.ToUnityScaled();
			Rotation = CalcQuaternion(pLeapHand.Basis);
			Radius = 0.01f;

			var cursor = new LeapInputCursor(IsLeft);

			foreach ( Finger leapFinger in pLeapHand.Fingers ) {
				if ( leapFinger == null || !leapFinger.IsValid ) {
					continue;
				}

				cursor.Rebuild(leapFinger);

				Rotation = Quaternion.Slerp(Rotation, cursor.Rotation, 0.1f);
				Radius = Math.Max(Radius, (cursor.Position-Position).sqrMagnitude);
			}

			Radius = (float)Math.Sqrt(Radius);

			NavigateBackStrength = pLeapHand.GrabStrength/pSettings.NavBackGrabThreshold;
			NavigateBackStrength = Mathf.Clamp(NavigateBackStrength, 0, 1);

			DisplayStrength = Vector3.Dot(pLeapHand.PalmNormal.ToUnity(), pSettings.PalmDirection);
			DisplayStrength = Mathf.Clamp((DisplayStrength-0.7f)/0.25f, 0, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static Quaternion CalcQuaternion(Matrix pBasis) {
			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html

			float[] mat = pBasis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);
			return Quaternion.LookRotation(column2, column1);
		}

	}

}
