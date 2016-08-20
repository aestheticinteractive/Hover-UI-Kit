using System.Collections.Generic;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	public enum CursorType {
		LeftPalm,
		LeftThumb,
		LeftIndex,
		LeftMiddle,
		LeftRing,
		LeftPinky,

		RightPalm,
		RightThumb,
		RightIndex,
		RightMiddle,
		RightRing,
		RightPinky,

		Look
	}


	/*================================================================================================*/
	public struct CursorTypeComparer : IEqualityComparer<CursorType> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool Equals(CursorType pTypeA, CursorType pTypeB) {
			return pTypeA == pTypeB;
		}

		/*--------------------------------------------------------------------------------------------*/
		public int GetHashCode(CursorType pType) {
			return (int)pType;
		}

	}

}
