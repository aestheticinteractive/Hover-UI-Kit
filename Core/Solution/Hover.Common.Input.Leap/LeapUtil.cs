using System;
using Leap;
using UnityEngine;

namespace Hover.Common.Input.Leap {

	/*================================================================================================*/
	public static class LeapUtil {

		public const float InputScale = 0.001f;

		private static readonly Vector LeapUp = new Vector(0, 1, 0);
		private static readonly Vector LeapForward = new Vector(0, 0, 1);


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
			//Adapted from "LeapUnityExtensions.cs" in the Leap Motion SDK
			//Smaller GC allocations than the previous "ToMatrix4x4()" approach
			Vector3 up = pBasis.TransformDirection(LeapUp).ToUnity(); //GC_ALLOC
			Vector3 forward = pBasis.TransformDirection(LeapForward).ToUnity(); //GC_ALLOC
			return Quaternion.LookRotation(forward, up);
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

		/*--------------------------------------------------------------------------------------------*/
		public static Frame GetValidLeapFrame(Controller pLeapControl) {
			Frame frame = pLeapControl.Frame(0); //GC_ALLOC
			return (frame != null && frame.IsValid ? frame : null);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Hand GetValidLeapHand(Frame pLeapFrame, bool pIsLeft) {
			if ( pLeapFrame == null ) {
				return null;
			}

			HandList hands = pLeapFrame.Hands; //GC_ALLOC

			for ( int i = 0 ; i < hands.Count ; i++ ) { //GC_ALLOC (get "Count")
				Hand leapHand = hands[i];

				if ( leapHand.IsLeft == pIsLeft && leapHand.IsValid ) {
					return leapHand;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Finger GetValidFinger(Hand pHand, Finger.FingerType pFingerType) {
			//Skip "Fingers.FingerType()" API method to avoid GC allocations
			FingerList fingers = pHand.Fingers; //GC_ALLOC

			for ( int i = 0 ; i < fingers.Count ; i++ ) {
				Finger finger = fingers[i]; //GC_ALLOC

				if ( finger.Type == pFingerType && finger.IsValid ) {
					return finger;
				}
			}

			return null;
		}

	}

}
