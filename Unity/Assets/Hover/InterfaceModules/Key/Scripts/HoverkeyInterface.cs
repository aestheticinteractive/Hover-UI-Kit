using System;
using System.Collections.Generic;
using Hover.Core.Items;
using Hover.Core.Items.Types;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	public class HoverkeyInterface : MonoBehaviour {

		[Serializable]
		public class HoverkeySelectedEvent : UnityEvent<IItemDataSelectable, HoverkeyItemLabels> {}

		[Serializable]
		public class HoverkeyToggledEvent : UnityEvent<IItemDataSelectable<bool>, HoverkeyItemLabels> {}

		public HoverkeySelectedEvent OnItemSelectedEvent = new HoverkeySelectedEvent();
		public HoverkeySelectedEvent OnItemDeselectedEvent = new HoverkeySelectedEvent();
		public HoverkeyToggledEvent OnItemToggledEvent = new HoverkeyToggledEvent();

		private List<HoverkeyItemLabels> vAllLabels;
		private IItemDataSticky vShiftStickyL;
		private IItemDataSticky vShiftStickyR;
		private IItemDataCheckbox vCapsCheckbox;
		private bool vWasShiftMode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			RefreshKeyLists();
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

		/*--------------------------------------------------------------------------------------------*/
		public void RefreshKeyLists() {
			FillLabelLists();
			FillShiftData();
			UpdateShiftLabels(true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelected(IItemDataSelectable pItemData) {
			OnItemSelectedEvent.Invoke(pItemData, GetLabels(pItemData));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemDeselected(IItemDataSelectable pItemData) {
			OnItemDeselectedEvent.Invoke(pItemData, GetLabels(pItemData));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChanged(IItemDataSelectable<bool> pItemData) {
			OnItemToggledEvent.Invoke(pItemData, GetLabels(pItemData));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HoverkeyItemLabels GetLabels(IItemDataSelectable pItemData) {
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
						vShiftStickyL = label.GetComponent<HoverItemDataSticky>();
						break;

					case KeyCode.RightShift:
						vShiftStickyR = label.GetComponent<HoverItemDataSticky>();
						break;

					case KeyCode.CapsLock:
						vCapsCheckbox = label.GetComponent<HoverItemDataCheckbox>();
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

			for ( int i = 0 ; i < vAllLabels.Count ; i++ ) {
				HoverkeyItemLabels label = vAllLabels[i];
				label.GetComponent<HoverItemData>().Label =
					(isShiftMode && label.HasShiftLabel ? label.ShiftLabel : label.DefaultLabel);
			}
		}

	}

}
