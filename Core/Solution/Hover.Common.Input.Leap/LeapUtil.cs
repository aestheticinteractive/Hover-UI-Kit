using System;
using Leap;
using UnityEngine;

namespace Hover.Common.Input.Leap {

	/*================================================================================================*/
	public static class LeapUtil {

		public const float InputScale = 0.001f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnity(this Vector pLeapVector) {
			return new Vector3(pLeapVector.x, pLeapVector.y, -pLeapVector.z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnityScaled(this Vector pLeapVector) {
			return ToUnity(pLeapVector)*InputScale;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Quaternion CalcQuaternion(Matrix pBasis) {
			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html

			float[] mat = pBasis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);
			return Quaternion.LookRotation(column2, column1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Finger.FingerType? GetFingerType(CursorType pCursorType) {
			switch ( pCursorType ) {
				case CursorType.LeftThumb:
				case CursorType.RightThumb:
					return Finger.FingerType.TYPE_THUMB;

				case CursorType.LeftIndex:
				case CursorType.RightIndex:
					return Finger.FingerType.TYPE_INDEX;

				case CursorType.LeftMiddle:
				case CursorType.RightMiddle:
					return Finger.FingerType.TYPE_MIDDLE;

				case CursorType.LeftRing:
				case CursorType.RightRing:
					return Finger.FingerType.TYPE_RING;

				case CursorType.LeftPinky:
				case CursorType.RightPinky:
					return Finger.FingerType.TYPE_PINKY;

				case CursorType.LeftPalm:
				case CursorType.RightPalm:
					return null;
			}

			throw new Exception("Unhandled CursorType: "+pCursorType);
		}
	}

}
