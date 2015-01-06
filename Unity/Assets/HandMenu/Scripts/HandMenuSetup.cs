using System.Linq;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandMenuSetup : MonoBehaviour {

		private GameObject vHandControlObj;
		private Controller vControl;
		private HandDisplay vHandL;
		private HandDisplay vHandR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHandControlObj = GameObject.Find("HandController");
			
			var handLObj = new GameObject("HandDisplayL");
			SetAndMoveToParent(handLObj.transform, vHandControlObj.transform);
			vHandL = handLObj.AddComponent<HandDisplay>();
			vHandL.IsLeft = true;

			var handRObj = new GameObject("HandDisplayR");
			SetAndMoveToParent(handRObj.transform, vHandControlObj.transform);
			vHandR = handRObj.AddComponent<HandDisplay>();
			vHandR.IsLeft = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vControl = vHandControlObj.GetComponent<HandController>().GetLeapController();
			vHandL.GetCurrentHand = (() => GetHand(true));
			vHandR.GetCurrentHand = (() => GetHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			OVRManager.capiHmd.DismissHSWDisplay();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void SetAndMoveToParent(Transform pChild, Transform pParent) {
			pChild.parent = pParent;
			pChild.position = pParent.position;
			pChild.rotation = pParent.rotation;
			pChild.localScale = Vector3.one;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Hand GetHand(bool pIsLeft) {
			return vControl.Frame().Hands
				.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
