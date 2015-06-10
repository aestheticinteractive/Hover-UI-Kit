using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class CacheList<T> where T : new() {

		public ReadOnlyCollection<T> ReadOnly { get; private set; }

		private readonly List<T> vCache;
		private readonly List<T> vList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CacheList() {
			vCache = new List<T>();
			vList = new List<T>();
			ReadOnly = new ReadOnlyCollection<T>(vList);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Clear() {
			vList.Clear();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Add(T pItem) {
			vList.Add(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddRange(IEnumerable<T> pItems) {
			vList.AddRange(pItems);
		}

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
