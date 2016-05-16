using System;
using System.Collections.Generic;
using System.Linq;

namespace Hover.Common.Input {

	/*================================================================================================* /
	public static class CursorTypeUtil {

		public static readonly CursorType[] AllCursorTypes = Enum.GetValues(typeof(CursorType))
			.Cast<CursorType>()
			.ToArray();

		private static readonly ListMap<CursorType, bool> ExcludeMap = 
			new ListMap<CursorType, bool>(EnumIntKeyComparer.CursorType, false, true);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
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

		/*--------------------------------------------------------------------------------------------* /
		public static bool IsPalm(CursorType pCursorType) {
			switch ( pCursorType ) {
				case CursorType.LeftPalm:
				case CursorType.RightPalm:
					return true;
			}

			return false;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public static void Exclude(IList<CursorType> pBaseTypes, IList<CursorType> pRemoveTypes,
																	IList<CursorType> pResultTypes) {
			pResultTypes.Clear();
			ExcludeMap.Clear();

			for ( int i = 0 ; i < pBaseTypes.Count ; i++ ) {
				ExcludeMap[pBaseTypes[i]] = true;
			}

			for ( int i = 0 ; i < pRemoveTypes.Count ; i++ ) {
				ExcludeMap[pRemoveTypes[i]] = false;
			}

			for ( int i = 0 ; i < ExcludeMap.KeysReadOnly.Count ; i++ ) {
				CursorType type = ExcludeMap.KeysReadOnly[i];

				if ( ExcludeMap[type] ) {
					pResultTypes.Add(type);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------* /
		public static bool Contains(IList<CursorType> pList, CursorType pType) {
			for ( int i = 0 ; i < pList.Count ; i++ ) {
				if ( pList[i] == pType ) {
					return true;
				}
			}

			return false;
		}

	}*/

}
