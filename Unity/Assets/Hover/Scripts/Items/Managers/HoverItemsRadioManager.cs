using System;
using System.Collections.Generic;
using Hover.Items.Types;
using UnityEngine;

namespace Hover.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsRadioManager : MonoBehaviour {

		private List<HoverItem> vItemsBuffer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vItemsBuffer = new List<HoverItem>();

			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();

			itemsMan.OnItemListInitialized.AddListener(AddAllItemListeners);
			itemsMan.OnItemAdded.AddListener(AddItemListeners);
			itemsMan.OnItemRemoved.AddListener(RemoveItemListeners);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddAllItemListeners() {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();

			itemsMan.FillListWithAllItems(vItemsBuffer);

			foreach ( HoverItem item in vItemsBuffer ) {
				AddItemListeners(item);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AddItemListeners(HoverItem pItem) {
			AddRadioDataListeners(pItem);
			pItem.OnTypeChanged += AddRadioDataListeners;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveItemListeners(HoverItem pItem) {
			pItem.OnTypeChanged -= AddRadioDataListeners;
			RemoveRadioDataListeners(pItem);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddRadioDataListeners(HoverItem pItem) {
			IRadioItemData radData = (pItem.Data as IRadioItemData);

			if ( radData != null ) {
				radData.OnValueChanged += HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveRadioDataListeners(HoverItem pItem) {
			IRadioItemData radData = (pItem.Data as IRadioItemData);

			if ( radData != null ) {
				radData.OnValueChanged -= HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleRadioValueChanged(ISelectableItemData<bool> pSelData) {
			IRadioItemData radData = (IRadioItemData)pSelData;

			if ( !radData.Value ) {
				return;
			}

			DeselectRemainingRadioGroupMembers(radData);
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRemainingRadioGroupMembers(IRadioItemData pRadioData) {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();
			string groupId = pRadioData.GroupId;

			Func<HoverItem, bool> filter = (tryItem => {
				IRadioItemData match = (tryItem.Data as IRadioItemData);
				return (match != null && match != pRadioData && match.GroupId == groupId);
			});

			itemsMan.FillListWithMatchingItems(vItemsBuffer, filter);

			for ( int i = 0 ; i < vItemsBuffer.Count ; i++ ) {
				((IRadioItemData)vItemsBuffer[i].Data).Value = false;
			}
		}

	}

}
