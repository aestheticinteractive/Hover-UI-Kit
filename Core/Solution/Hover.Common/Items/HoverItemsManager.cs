using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public class HoverItemsManager : MonoBehaviour {

		[Serializable]
		public class ItemEvent : UnityEvent<HoverItemData> {}

		public UnityEvent OnItemListInitialized;
		public ItemEvent OnItemAdded;
		public ItemEvent OnItemRemoved;

		private List<HoverItemData> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItems = FindObjectsOfType<HoverItemData>().ToList();
			OnItemListInitialized.Invoke();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(HoverItemData pItem) {
			if ( vItems == null ) {
				return;
			}

			if ( vItems.Contains(pItem) ) {
				Debug.LogWarning("Cannot add duplicate item '"+pItem.name+"'.", pItem);
				return;
			}
		
			vItems.Add(pItem);
			OnItemAdded.Invoke(pItem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void RemoveItem(HoverItemData pItem) {
			if ( vItems == null ) {
				return;
			}

			if ( !vItems.Remove(pItem) ) {
				Debug.LogWarning("Cannot remove missing item '"+pItem.name+"'.", pItem);
				return;
			}

			OnItemRemoved.Invoke(pItem);
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithExistingItemComponents<T>(IList<T> pComponents) where T : Component {
			pComponents.Clear();
			
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				T comp = vItems[i].GetComponent<T>();
				
				if ( comp != null  ) {
					pComponents.Add(comp);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithAllItems(IList<HoverItemData> pItems) {
			pItems.Clear();
			
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				pItems.Add(vItems[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithMatchingItems(IList<HoverItemData> pMatches, 
																Func<HoverItemData, bool> pFilterFunc) {
			pMatches.Clear();
			
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItemData item = vItems[i];
				
				if ( pFilterFunc(item) ) {
					pMatches.Add(item);
				}
			}
		}

	}

}
