using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	public static class CursorUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool FindCursorReference(MonoBehaviour pModule,
													ref HoverCursorDataProvider pProv, bool pShowLog) {
			if ( pProv != null ) {
				return true;
			}

			pProv = Object.FindObjectOfType<HoverCursorDataProvider>();

			if ( pShowLog ) {
				string typeName = typeof(HoverCursorDataProvider).Name;

				if ( pProv == null ) {
					Debug.LogWarning("Could not find '"+typeName+"' reference.", pModule);
				}
				else {
					Debug.Log("Found '"+typeName+"' reference.", pModule);
				}
			}

			return (pProv != null);
		}

	}

}
