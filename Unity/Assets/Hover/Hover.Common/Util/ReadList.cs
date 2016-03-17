using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class ReadList<T> {

		public ReadOnlyCollection<T> ReadOnly { get; private set; }

		protected readonly List<T> vList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ReadList() {
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

	}

}
