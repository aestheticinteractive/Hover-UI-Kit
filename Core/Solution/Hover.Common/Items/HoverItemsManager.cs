using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public class HoverItemsManager : MonoBehaviour {

		private List<HoverItemData> vItems;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemsManager() {
			vItems = new List<HoverItemData>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItems = FindObjectsOfType<HoverItemData>().ToList();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(HoverItemData pItem) {
			if ( vItems.Contains(pItem) ) {
				Debug.LogWarning("Cannot add duplicate item '"+pItem.name+"'.", pItem);
				return;
			}
		
			vItems.Add(pItem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void RemoveItem(HoverItemData pItem) {
			if ( !vItems.Remove(pItem) ) {
				Debug.LogWarning("Cannot remove missing item '"+pItem.name+"'.", pItem);
				return;
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithExistingItemComponents<T>(IList<T> pComponents) where T : Component {
			pComponents.Clear();

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				T comp = vItems[i].GetComponent<T>();
				
				if ( comp != null  ) {
					pComponents.Add(comp);
				}
			}
		}

	}

}
