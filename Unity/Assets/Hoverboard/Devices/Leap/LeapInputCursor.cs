using System.Linq;
using Hoverboard.Core.Input;
using Leap;
using UnityEngine;

namespace Hoverboard.Devices.Leap {

	/*================================================================================================*/
	public class LeapInputCursor : IInputCursor {

		public CursorType Type { get; private set; }
		public bool IsAvailable { get; private set; }
		public bool IsEnabled { get; set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }

		private readonly Finger.FingerType vLeapFingerType;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputCursor(CursorType pType, Finger.FingerType pFingerType) {
			Type = pType;
			vLeapFingerType = pFingerType;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Rebuild(Hand pLeapHand) {
			Finger leapFinger = null;

			if ( pLeapHand != null ) {
				leapFinger = pLeapHand.Fingers
					.FingerType(vLeapFingerType)
					.FirstOrDefault(f => f.IsValid);
			}

			if ( leapFinger == null ) {
				IsAvailable = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				return;
			}


			Bone bone = leapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			IsAvailable = true;
			Position = leapFinger.TipPosition.ToUnityScaled();
			Rotation = CalcQuaternion(bone.Basis);
			Size = leapFinger.Width/160f;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Quaternion CalcQuaternion(Matrix pBasis) {
			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html

			float[] mat = pBasis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);
			return Quaternion.LookRotation(column2, column1);
		}

	}

}
