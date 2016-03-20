using System;
using System.Collections.Generic;
using Leap;

namespace Hover.Common.Input.Leap {

	/*================================================================================================*/
	public static class LeapUtil {


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
		public static Hand GetValidLeapHand(Frame pLeapFrame, bool pIsLeft) {
			if ( pLeapFrame == null ) {
				return null;
			}

			List<Hand> hands = pLeapFrame.Hands; //GC_ALLOC

			for ( int i = 0 ; i < hands.Count ; i++ ) { //GC_ALLOC (get "Count")
				Hand leapHand = hands[i];

				if ( leapHand.IsLeft == pIsLeft ) {
					return leapHand;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Finger GetValidFinger(Hand pHand, Finger.FingerType pFingerType) {
			//Skip "Fingers.FingerType()" API method to avoid GC allocations
			List<Finger> fingers = pHand.Fingers; //GC_ALLOC

			for ( int i = 0 ; i < fingers.Count ; i++ ) {
				Finger finger = fingers[i]; //GC_ALLOC

				if ( finger.Type == pFingerType ) {
					return finger;
				}
			}

			return null;
		}

	}

}
