using System.Collections.Generic;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.Custom {

	/*================================================================================================*/
	public class CustomItemFinder<TItem, TCust> where TItem : MonoBehaviour, IHovercommonItem 
																		where TCust : MonoBehaviour {

		private readonly string vLogPrefix;
		private readonly IDictionary<int, TCust> vMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CustomItemFinder(string pLogPrefix) {
			vLogPrefix = pLogPrefix;
			vMap = new Dictionary<int, TCust>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FindAll() {
			TCust[] customList = Object.FindObjectsOfType<TCust>();

			foreach ( TCust custom in customList ) {
				TItem item = custom.gameObject.GetComponent<TItem>();

				if ( item == null ) {
					continue;
				}

				vMap.Add(item.GetItem().AutoId, custom);

				Debug.Log(vLogPrefix+" | Using the "+typeof(TCust).Name+" found in the '"+
					UnityUtil.GetPath(item.gameObject)+"' GameObject.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TCust GetCustom(int pItemAutoId) {
			return (vMap.ContainsKey(pItemAutoId) ? vMap[pItemAutoId] : null);
		}

	}

}
