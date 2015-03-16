using System.Collections.Generic;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public class ItemDatabase<T> : IItemDatabase<T> where T : class {

		private readonly IDictionary<int, T> vMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemDatabase() {
			vMap = new Dictionary<int, T>();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetData(IBaseItem pItem, T pData) {
			int key = pItem.AutoId;

			if ( !vMap.ContainsKey(key) ) {
				vMap.Add(key, pData);
			}
			else {
				vMap[key] = pData;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public T GetData(IBaseItem pItem) {
			int key = pItem.AutoId;
			return (vMap.ContainsKey(key) ? vMap[key] : null);
		}

	}

}
