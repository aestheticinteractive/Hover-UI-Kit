using HandMenu.Input;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandMenuSetup : MonoBehaviour {

		public bool LeftHandMenu = true;

		private HandController vHandControl;
		private Controller vLeapControl;
		private HandDisplay vHand;
		private InputProvider vInputProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			GameObject handControlObj = GameObject.Find("HandController");
			vHandControl = handControlObj.GetComponent<HandController>();
			vLeapControl = vHandControl.GetLeapController();
			vInputProv = new InputProvider();

			////

			var handObj = new GameObject("HandDisplay");
			SetAndMoveToParent(handObj.transform, handControlObj.transform);

			vHand = handObj.AddComponent<HandDisplay>();
			vHand.IsLeft = LeftHandMenu;
			vHand.MenuHandProvider = vInputProv.GetHandProvider(LeftHandMenu);
			vHand.SelectHandProvider = vInputProv.GetHandProvider(!LeftHandMenu);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				OVRManager.capiHmd.DismissHSWDisplay();
			}

			vInputProv.UpdateWithFrame(vLeapControl.Frame());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void SetAndMoveToParent(Transform pChild, Transform pParent) {
			pChild.parent = pParent;
			pChild.position = pParent.position;
			pChild.rotation = pParent.rotation;
			pChild.localScale = Vector3.one;
		}

	}

}
