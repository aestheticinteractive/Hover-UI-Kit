using System;
using System.Collections.Generic;

namespace Hover.Common.Util {

	/*================================================================================================*/
	// Code derived from: http://stackoverflow.com/a/26281533 and
	//   http://forum.muchdifferent.com/unitypark/index.php?p=/discussion/1793
	public class EnumIntKeyComparer<T> : IEqualityComparer<T> {

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
