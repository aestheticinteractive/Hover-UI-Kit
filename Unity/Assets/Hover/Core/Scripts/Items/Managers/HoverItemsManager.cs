using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	public class HoverItemsManager : MonoBehaviour {

		[Serializable]
		public class ItemEvent : UnityEvent<HoverItem> {}

		public UnityEvent OnItemListInitialized;
		public ItemEvent OnItemAdded;
		public ItemEvent OnItemRemoved;

		private List<HoverItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItems = Resources.FindObjectsOfTypeAll<HoverItem>().ToList();
			OnItemListInitialized.Invoke();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(HoverItem pItem) {
			if ( vItems == null ) {
				return;
			}

			if ( vItems.Contains(pItem) ) {
				//Debug.LogWarning("Cannot add duplicate item '"+pItem.name+"'.", pItem);
				return;
			}
		
			vItems.Add(pItem);
			OnItemAdded.Invoke(pItem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void RemoveItem(HoverItem pItem) {
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
		public void FillListWithAllItems(IList<HoverItem> pItems) {
			pItems.Clear();
			
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				pItems.Add(vItems[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithMatchingItems(IList<HoverItem> pMatches, 
																Func<HoverItem, bool> pFilterFunc) {
			pMatches.Clear();
			
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItem item = vItems[i];
				
				if ( pFilterFunc(item) ) {
					pMatches.Add(item);
				}
			}
		}

	}

}
