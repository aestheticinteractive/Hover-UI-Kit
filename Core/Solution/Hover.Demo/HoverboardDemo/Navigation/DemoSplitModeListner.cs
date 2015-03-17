using UnityEngine;

namespace Hover.Demo.HoverboardDemo.Navigation {

	/*================================================================================================*/
	public class DemoSplitModeListner : DemoBaseListener<NavItemCheckbox> {

		private Vector3[] vOrigPosList;
		private Quaternion[] vOrigRotList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			
			vOrigPosList = new Vector3[ItemPanels.Length];
			vOrigRotList = new Quaternion[ItemPanels.Length];

			for ( int i = 0 ; i < ItemPanels.Length ; i++ ) {
				Transform tx = ItemPanels[i].DisplayContainer.transform;
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
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			if ( !pNavItem.Value ) {
				Transform leftTx = ItemPanels[0].DisplayContainer.transform;
				Transform rightTx = ItemPanels[1].DisplayContainer.transform;

				leftTx.localPosition = new Vector3(0.25f, 0, 0);
				leftTx.localRotation = Quaternion.Euler(90, 0, 0);

				rightTx.localPosition = new Vector3(-0.25f, 0, 0);
				rightTx.localRotation = Quaternion.Euler(90, 0, 0);

				KeyboardObj.transform.localPosition = new Vector3(0, -0.1f, 0.15f);
				KeyboardObj.transform.localRotation = Quaternion.Euler(40, 0, 0);
				return;
			}

			for ( int i = 0 ; i < ItemPanels.Length ; i++ ) {
				Transform tx = ItemPanels[i].DisplayContainer.transform;
				tx.localPosition = vOrigPosList[i];
				tx.localRotation = vOrigRotList[i];
			}

			KeyboardObj.transform.localPosition = Vector3.zero;
			KeyboardObj.transform.localRotation = Quaternion.identity;
		}

	}

}
