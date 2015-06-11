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
		public ListMap(IEqualityComparer<TKey> pComparer) {
			vMap = new Dictionary<TKey, TValue>(pComparer);
			vKeys = new List<TKey>();
			vValues = new List<TValue>();

			KeysReadOnly = new ReadOnlyCollection<TKey>(vKeys);
			ValuesReadOnly = new ReadOnlyCollection<TValue>(vValues);
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
					vValues.Remove(val);
					vMap[pKey] = value;
				}
				else {
					vMap.Add(pKey, value);
					vKeys.Add(pKey);
				}

				vValues.Add(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Clear() {
			vMap.Clear();
			vKeys.Clear();
			vValues.Clear();
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
