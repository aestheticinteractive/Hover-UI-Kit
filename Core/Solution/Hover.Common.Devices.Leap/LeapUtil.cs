using System;
using Hover.Common.Input;
using Leap;
using UnityEngine;

namespace Hover.Common.Devices.Leap {

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
