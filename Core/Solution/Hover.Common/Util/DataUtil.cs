using System.Collections.Generic;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public static class DataUtil<T> {

		private static readonly HashSet<T> ExcludeMap = new HashSet<T>();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void Exclude(IList<T> pBase, IList<T> pRemove, IList<T> pResult) {
			pResult.Clear();
			ExcludeMap.Clear();

			for ( int i = 0 ; i < pBase.Count ; i++ ) {
				ExcludeMap.Add(pBase[i]);
			}

			for ( int i = 0 ; i < pRemove.Count ; i++ ) {
				ExcludeMap.Remove(pRemove[i]);
			}

			foreach ( T keepItem in ExcludeMap ) {
				pResult.Add(keepItem);
			}
		}

	}

}
