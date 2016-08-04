using System.Collections.Generic;
using Hover.Items;
using Hover.Layouts.Rect;
using Hover.Utils;
using UnityEngine;
using Hover.Items.Types;

namespace Hover.Interfaces.Key {

	/*================================================================================================*/
	public class HoverkeyManager : MonoBehaviour {

		public HoverLayoutRectRow Full;
		public HoverLayoutRectRow Arrows;
		public HoverLayoutRectRow Numpad;
		public HoverLayoutRectRow Functions;
		public HoverLayoutRectRow SixGroup;
		public HoverLayoutRectRow ThreeGroup;

		private List<HoverkeyItemLabels> vAllLabels;
		private List<HoverkeyItemLabels> vFullLabels;
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
		private void FillLabelLists() {
			vAllLabels = new List<HoverkeyItemLabels>();
			vFullLabels = new List<HoverkeyItemLabels>();

			GetComponentsInChildren(vAllLabels);
			Full.GetComponentsInChildren(vFullLabels);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FillShiftData() {
			foreach ( HoverkeyItemLabels label in vFullLabels ) {
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool IsInShiftMode() {
			return (vShiftStickyL.IsStickySelected || vShiftStickyR.IsStickySelected ||
				vCapsCheckbox.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateShiftLabels(bool pForceUpdate=false) {
			bool isShiftMode = IsInShiftMode();

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
