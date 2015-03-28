using System;
using System.Linq;

namespace Hover.Common.Custom {

	/*================================================================================================*/
	public static class CustomUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void VerifyRenderer<T>(Type pType, string pLabel, string pDomain, string pUnit) {
			if ( pType == null ) {
				throw new Exception(pDomain+" | The "+pUnit+"' renderer "+(pLabel == null ? "" : 
				"for '"+pLabel+"' ")+"cannot be null.");
			}

			if ( !pType.GetInterfaces().Contains(typeof(T)) ) {
				throw new Exception(pDomain+" | The "+pUnit+"' renderer "+(pLabel == null ? "" : 
				"for '"+pLabel+"' ")+"' does not implement the "+typeof(T).Name+" interface.");
			}
		}

	}

}
