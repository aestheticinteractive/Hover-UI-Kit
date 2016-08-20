using System;
using System.Collections.Generic;
using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Managers {

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

			for ( int i = 0 ; i < vItemsBuffer.Count ; i++ ) {
				AddItemListeners(vItemsBuffer[i]);
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
			IItemDataRadio radData = (pItem.Data as IItemDataRadio);

			if ( radData != null ) {
				radData.OnValueChanged += HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveRadioDataListeners(HoverItem pItem) {
			IItemDataRadio radData = (pItem.Data as IItemDataRadio);

			if ( radData != null ) {
				radData.OnValueChanged -= HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleRadioValueChanged(IItemDataSelectable<bool> pSelData) {
			IItemDataRadio radData = (IItemDataRadio)pSelData;

			if ( !radData.Value ) {
				return;
			}

			DeselectRemainingRadioGroupMembers(radData);
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRemainingRadioGroupMembers(IItemDataRadio pRadioData) {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();
			string groupId = pRadioData.GroupId;

			Func<HoverItem, bool> filter = (tryItem => {
				IItemDataRadio match = (tryItem.Data as IItemDataRadio);
				return (match != null && match != pRadioData && match.GroupId == groupId);
			});

			itemsMan.FillListWithMatchingItems(vItemsBuffer, filter);

			for ( int i = 0 ; i < vItemsBuffer.Count ; i++ ) {
				((IItemDataRadio)vItemsBuffer[i].Data).Value = false;
			}
		}

	}

}
