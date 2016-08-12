using System;
using System.Collections.Generic;
using Hover.Items;
using Hover.Items.Types;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Interfaces.Key {

	/*================================================================================================*/
	public class HoverkeyManager : MonoBehaviour {

		[Serializable]
		public class HoverkeySelectedEvent : UnityEvent<ISelectableItemData, HoverkeyItemLabels> {}

		[Serializable]
		public class HoverkeyToggledEvent : UnityEvent<ISelectableItemData<bool>, HoverkeyItemLabels> {}

		public HoverkeySelectedEvent OnItemSelectedEvent;
		public HoverkeySelectedEvent OnItemDeselectedEvent;
		public HoverkeyToggledEvent OnItemToggledEvent;

		private List<HoverkeyItemLabels> vAllLabels;
		private IStickyItemData vShiftStickyL;
		private IStickyItemData vShiftStickyR;
		private ICheckboxItemData vCapsCheckbox;
		private bool vWasShiftMode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			FillLabelLists();
			FillShiftData();
			UpdateShiftLabels(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateShiftLabels();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsLeftShiftSelected {
			get {
				return vShiftStickyL.IsStickySelected;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsRightShiftSelected {
			get {
				return vShiftStickyR.IsStickySelected;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsCapsLockActive {
			get {
				return vCapsCheckbox.Value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsInShiftMode {
			get {
				return (IsLeftShiftSelected || IsRightShiftSelected || IsCapsLockActive);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelected(ISelectableItemData pItemData) {
			OnItemSelectedEvent.Invoke(pItemData, GetLabels(pItemData));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemDeselected(ISelectableItemData pItemData) {
			OnItemDeselectedEvent.Invoke(pItemData, GetLabels(pItemData));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChanged(ISelectableItemData<bool> pItemData) {
			OnItemToggledEvent.Invoke(pItemData, GetLabels(pItemData));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HoverkeyItemLabels GetLabels(ISelectableItemData pItemData) {
			return pItemData.gameObject.GetComponent<HoverkeyItemLabels>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FillLabelLists() {
			vAllLabels = new List<HoverkeyItemLabels>();
			GetComponentsInChildren(vAllLabels);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FillShiftData() {
			foreach ( HoverkeyItemLabels label in vAllLabels ) {
				switch ( label.DefaultKey ) {
					case KeyCode.LeftShift:
						vShiftStickyL = (label.GetComponent<HoverItemData>() as IStickyItemData);
						break;

					case KeyCode.RightShift:
						vShiftStickyR = (label.GetComponent<HoverItemData>() as IStickyItemData);
						break;

					case KeyCode.CapsLock:
						vCapsCheckbox = (label.GetComponent<HoverItemData>() as ICheckboxItemData);
						break;
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateShiftLabels(bool pForceUpdate=false) {
			bool isShiftMode = IsInShiftMode;

			if (  isShiftMode == vWasShiftMode && !pForceUpdate ) {
				return;
			}

			vWasShiftMode = isShiftMode;

			foreach ( HoverkeyItemLabels label in vAllLabels ) {
				label.GetComponent<HoverItemData>().Label = 
					(isShiftMode && label.HasShiftLabel ? label.ShiftLabel : label.DefaultLabel);
			}
		}

	}

}
