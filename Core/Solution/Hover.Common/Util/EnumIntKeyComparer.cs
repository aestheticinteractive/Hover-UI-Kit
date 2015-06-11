using System;
using System.Collections.Generic;
using Hover.Common.Input;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class EnumIntKeyComparer {

		public static readonly EnumIntKeyComparer<CursorType> CursorType = 
			new EnumIntKeyComparer<CursorType>(((a, b) => (a == b)), (a => (int)a));

	}


	/*================================================================================================*/
	// Code derived from: http://stackoverflow.com/a/26281533 and
	//   http://forum.muchdifferent.com/unitypark/index.php?p=/discussion/1793
	public class EnumIntKeyComparer<T> : EnumIntKeyComparer, IEqualityComparer<T> {

		private readonly Func<T, T, bool> vEqualsFunc;
		private readonly Func<T, int> vHashFunc;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public EnumIntKeyComparer(Func<T, T, bool> pEqualsFunc, Func<T, int> pHashFunc) {
			vEqualsFunc = pEqualsFunc;
			vHashFunc = pHashFunc;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool Equals(T pEnumA, T pEnumB) {
			return vEqualsFunc(pEnumA, pEnumB);
		}

		/*--------------------------------------------------------------------------------------------*/
		public int GetHashCode(T pEnum) {
			return vHashFunc(pEnum);
		}

	}

}
