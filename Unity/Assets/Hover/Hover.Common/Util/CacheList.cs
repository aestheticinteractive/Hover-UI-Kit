using System;
using System.Collections.Generic;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class CacheList<T> : ReadList<T> where T : new() {

		protected readonly List<T> vCache;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CacheList() {
			vCache = new List<T>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void RebuildWith<TParam>(IList<TParam> pParamItems, Action<T, TParam> pInitItemFunc) {
			vCache.AddRange(vList);
			vList.Clear();

			for ( int i = 0 ; i < pParamItems.Count ; i++ ) {
				T item;
				
				if ( vCache.Count > 0 ) {
					item = vCache[0];
					vCache.RemoveAt(0);
				}
				else {
					item = new T();
				}

				pInitItemFunc(item, pParamItems[i]);
				vList.Add(item);
			}
		}

	}

}
