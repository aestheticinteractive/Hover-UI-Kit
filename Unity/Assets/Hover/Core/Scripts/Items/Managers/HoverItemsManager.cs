using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	public class HoverItemsManager : MonoBehaviour {

		private static HoverItemsManager InstanceRef;

		[Serializable]
		public class ItemEvent : UnityEvent<HoverItem> {}

		public UnityEvent OnItemListInitialized;
		public ItemEvent OnItemAdded;
		public ItemEvent OnItemRemoved;

		private List<HoverItem> vItems;
		private List<HoverItem> vItemsActive;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static HoverItemsManager Instance {
			get {
				if ( InstanceRef == null ) {
					InstanceRef = FindObjectOfType<HoverItemsManager>();
				}

				return InstanceRef;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItems = Resources.FindObjectsOfTypeAll<HoverItem>().ToList();
			vItemsActive = new List<HoverItem>();

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				HoverItem item = vItems[i];

				if ( !item.isActiveAndEnabled || !item.gameObject.activeInHierarchy ) {
					continue;
				}

				vItemsActive.Add(item);
			}

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
				//Debug.LogWarning("Cannot remove missing item '"+pItem.name+"'.", pItem);
				return;
			}

			OnItemRemoved.Invoke(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetItemActiveState(HoverItem pItem, bool pIsActive) {
			if ( vItemsActive == null ) {
				return;
			}

			bool wasActive = vItemsActive.Contains(pItem);

			if ( pIsActive == wasActive ) {
				return;
			}

			if ( pIsActive ) {
				vItemsActive.Add(pItem);
			}
			else {
				vItemsActive.Remove(pItem);
			}

			//Debug.Log(Time.frameCount+" | SetItemActiveState: "+pIsActive+" / "+vItemsActive.Count);
		}

		/*--------------------------------------------------------------------------------------------* /
		public void RemoveDestroyedItems() {
			if ( vItems == null ) {
				return;
			}

			for ( int i = 0 ; i < vItems.Count ; i++ ) {
				if ( vItems[i] == null ) {
					Debug.LogWarning("Removing destroyed item @ "+i);
					vItems.RemoveAt(i);
					i--;
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithActiveItemComponents<T>(IList<T> pComponents) where T : Component {
			pComponents.Clear();

			if ( vItemsActive == null ) {
				return;
			}

			int itemCount = vItemsActive.Count;

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverItem item = vItemsActive[i];

				if ( item == null ) {
					continue;
				}

				T comp = item.GetComponent<T>();
				
				if ( comp != null ) {
					pComponents.Add(comp);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithAllItems(IList<HoverItem> pItems, bool pActiveOnly) {
			pItems.Clear();

			List<HoverItem> items = (pActiveOnly ? vItemsActive : vItems);

			if ( items == null ) {
				return;
			}

			int itemCount = items.Count;

			for ( int i = 0 ; i < itemCount ; i++ ) {
				pItems.Add(items[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithMatchingItems(IList<HoverItem> pMatches, bool pActiveOnly,
																Func<HoverItem, bool> pFilterFunc) {
			pMatches.Clear();

			List<HoverItem> items = (pActiveOnly ? vItemsActive : vItems);

			if ( items == null ) {
				return;
			}

			int itemCount = items.Count;

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverItem item = items[i];
				
				if ( pFilterFunc(item) ) {
					pMatches.Add(item);
				}
			}
		}

	}

}
