using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public class DemoSplitModeListner : DemoBaseListener<NavItemCheckbox> {

		private Vector3[] vOrigPosList;
		private Quaternion[] vOrigRotList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			
			vOrigPosList = new Vector3[NavPanels.Length];
			vOrigRotList = new Quaternion[NavPanels.Length];

			for ( int i = 0 ; i < NavPanels.Length ; i++ ) {
				Transform tx = NavPanels[i].Container.transform;
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
				Transform leftTx = NavPanels[0].Container.transform;
				Transform rightTx = NavPanels[1].Container.transform;

				leftTx.localPosition = new Vector3(0.25f, 0, 0);
				leftTx.localRotation = Quaternion.Euler(90, 0, 0);

				rightTx.localPosition = new Vector3(-0.25f, 0, 0);
				rightTx.localRotation = Quaternion.Euler(90, 0, 0);

				KeyboardObj.transform.localPosition = new Vector3(0, -0.1f, 0.15f);
				KeyboardObj.transform.localRotation = Quaternion.Euler(40, 0, 0);
				return;
			}

			for ( int i = 0 ; i < NavPanels.Length ; i++ ) {
				Transform tx = NavPanels[i].Container.transform;
				tx.localPosition = vOrigPosList[i];
				tx.localRotation = vOrigRotList[i];
			}

			KeyboardObj.transform.localPosition = Vector3.zero;
			KeyboardObj.transform.localRotation = Quaternion.identity;
		}

	}

}
