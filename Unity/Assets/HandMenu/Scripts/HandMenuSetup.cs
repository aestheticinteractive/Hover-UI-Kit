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
			handLObj.transform.parent = vHandControlObj.transform;
			handLObj.transform.position = vHandControlObj.transform.position;
			handLObj.transform.rotation = vHandControlObj.transform.rotation;
			vHandL = handLObj.AddComponent<HandDisplay>();

			var handRObj = new GameObject("HandDisplayR");
			handRObj.transform.parent = vHandControlObj.transform;
			handRObj.transform.position = vHandControlObj.transform.position;
			handRObj.transform.rotation = vHandControlObj.transform.rotation;
			vHandR = handRObj.AddComponent<HandDisplay>();
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
		private Hand GetHand(bool pIsLeft) {
			return vControl.Frame().Hands
				.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
