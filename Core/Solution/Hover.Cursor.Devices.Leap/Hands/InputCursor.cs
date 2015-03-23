using System;
using System.Linq;
using Hover.Common.Devices.Leap;
using Hover.Common.Input;
using Hover.Cursor.Input;
using Leap;
using UnityEngine;

namespace Hover.Cursor.Devices.Leap.Hands {

	/*================================================================================================*/
	public class InputCursor : IInputCursor {

		private const float SizeScaleFactor = 1/160f;

		public CursorType Type { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }

		private readonly Finger.FingerType? vLeapFingerType;
		private readonly bool vIsPalm;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputCursor(CursorType pType) {
			Type = pType;
			vLeapFingerType = LeapUtil.GetFingerType(pType);
			vIsPalm = CursorTypeUtil.IsPalm(pType);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Rebuild(Hand pLeapHand) {
			if ( vLeapFingerType != null ) {
				RebuildForFinger(pLeapHand, (Finger.FingerType)vLeapFingerType);
			}
			else if ( vIsPalm ) {
				RebuildForPalm(pLeapHand);
			}
			else {
				throw new Exception("Unhandled CursorType: "+Type);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RebuildForPalm(Hand pLeapHand) {
			if ( pLeapHand == null ) {
				RebuildReset();
				return;
			}

			IsAvailable = true;
			Position = pLeapHand.PalmPosition.ToUnityScaled();
			Rotation = CalcQuaternion(pLeapHand.Basis);
			Size = pLeapHand.PalmWidth*SizeScaleFactor;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RebuildForFinger(Hand pLeapHand, Finger.FingerType pLeapFingerType) {
			Finger leapFinger = null;

			if ( pLeapHand != null ) {
				leapFinger = pLeapHand.Fingers
					.FingerType(pLeapFingerType)
					.FirstOrDefault(f => f.IsValid);
			}

			if ( leapFinger == null ) {
				RebuildReset();
				return;
			}

			Bone bone = leapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			IsAvailable = true;
			Position = leapFinger.TipPosition.ToUnityScaled();
			Rotation = CalcQuaternion(bone.Basis);
			Size = leapFinger.Width*SizeScaleFactor;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RebuildReset() {
			IsAvailable = false;
			Position = Vector3.zero;
			Rotation = Quaternion.identity;
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
