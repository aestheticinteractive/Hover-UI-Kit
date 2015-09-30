using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.BoardKeys.CastItems {

	/*================================================================================================*/
	public class DemoSplitModeListener : DemoBaseListener<ICheckboxItem> {

		private GameObject vKeyboardObj;
		private Vector3[] vOrigPosList;
		private Quaternion[] vOrigRotList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();

			vKeyboardObj = GameObject.Find("SplitKeyboard");
			vOrigPosList = new Vector3[ItemPanels.Length];
			vOrigRotList = new Quaternion[ItemPanels.Length];

			for ( int i = 0 ; i < ItemPanels.Length ; i++ ) {
				Transform tx = GetTransform(ItemPanels[i].DisplayContainer);
				vOrigPosList[i] = tx.localPosition;
				vOrigRotList[i] = tx.localRotation;
			}

			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<bool> pItem) {
			if ( !pItem.Value ) {
				for ( int i = 0 ; i < ItemPanels.Length ; ++i ) {
					Transform tx = GetTransform(ItemPanels[i].DisplayContainer);
					tx.localPosition = new Vector3(0.25f*Math.Sign(vOrigPosList[i].x), 0, 0);
					tx.localRotation = Quaternion.Euler(90, 0, 0);
				}

				vKeyboardObj.transform.localPosition = new Vector3(0, -0.05f, 0.1f);
				vKeyboardObj.transform.localRotation = Quaternion.Euler(40, 0, 0);
				return;
			}

			for ( int i = 0 ; i < ItemPanels.Length ; i++ ) {
				Transform tx = GetTransform(ItemPanels[i].DisplayContainer);
				tx.localPosition = vOrigPosList[i];
				tx.localRotation = vOrigRotList[i];
			}

			vKeyboardObj.transform.localPosition = Vector3.zero;
			vKeyboardObj.transform.localRotation = Quaternion.identity;
		}

		/*--------------------------------------------------------------------------------------------*/
		private Transform GetTransform(object pDisplayContainer) {
			return ((GameObject)pDisplayContainer).transform;
		}

	}

}
