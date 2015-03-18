using System;

namespace Hover.Common.Input {

	/*================================================================================================*/
	public static class CursorTypeUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsLeft(CursorType pCursorType) {
			switch ( pCursorType ) {
				case CursorType.LeftPalm:
				case CursorType.LeftThumb:
				case CursorType.LeftIndex:
				case CursorType.LeftMiddle:
				case CursorType.LeftRing:
				case CursorType.LeftPinky:
					return true;

				case CursorType.RightPalm:
				case CursorType.RightThumb:
				case CursorType.RightIndex:
				case CursorType.RightMiddle:
				case CursorType.RightRing:
				case CursorType.RightPinky:
					return false;
			}

			throw new Exception("Unhandled CursorType: "+pCursorType);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static bool IsPalm(CursorType pCursorType) {
			switch ( pCursorType ) {
				case CursorType.LeftPalm:
				case CursorType.RightPalm:
					return true;
			}

			return false;
		}
	
	}

}
