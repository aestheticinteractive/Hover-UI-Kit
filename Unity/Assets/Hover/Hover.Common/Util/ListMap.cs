using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class ListMap<TKey, TValue> {

		public ReadOnlyCollection<TKey> KeysReadOnly { get; private set; }
		public ReadOnlyCollection<TValue> ValuesReadOnly { get; private set; }

		private readonly Dictionary<TKey, TValue> vMap;
		private readonly List<TKey> vKeys;
		private readonly List<TValue> vValues;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ListMap(IEqualityComparer<TKey> pComparer, bool pSkipKeys=false, bool pSkipValues=false){
			vMap = new Dictionary<TKey, TValue>(pComparer);

			if ( !pSkipKeys ) {
				vKeys = new List<TKey>();
				KeysReadOnly = new ReadOnlyCollection<TKey>(vKeys);
			}

			if ( !pSkipValues ) {
				vValues = new List<TValue>();
				ValuesReadOnly = new ReadOnlyCollection<TValue>(vValues);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TValue this[TKey pKey] {
			get {
				return vMap[pKey];
			}
			set {
				TValue val;

				if ( vMap.TryGetValue(pKey, out val) ) {
					if ( vValues != null ) {
						vValues.Remove(val);
					}

					vMap[pKey] = value;
				}
				else {
					vMap.Add(pKey, value);

					if ( vKeys != null ) {
						vKeys.Add(pKey);
					}
				}

				if ( vValues != null ) {
					vValues.Add(value);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Clear() {
			vMap.Clear();

			if ( vKeys != null ) {
				vKeys.Clear();
			}

			if ( vValues != null ) {
				vValues.Clear();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Add(TKey pKey, TValue pValue) {
			this[pKey] = pValue;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool ContainsKey(TKey pKey) {
			return vMap.ContainsKey(pKey);
		}

		/*--------------------------------------------------------------------------------------------*/
		public TValue GetValue(Func<TValue, TValue, bool> pShouldRetainFirstValue) {
			TValue minVal = default(TValue);
			bool isFirst = true;

			foreach ( TValue val in vValues ) {
				if ( isFirst || pShouldRetainFirstValue(val, minVal) ) {
					isFirst = false;
					minVal = val;
				}
			}

			return minVal;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool HasValue(Func<TValue, bool> pPredicate) {
			foreach ( TValue val in vValues ) {
				if ( pPredicate(val) ) {
					return true;
				}
			}

			return false;
		}

	}

}
