using UnityEngine;
using UnityEngine.VR;

namespace Hover.Demo.Common {

	/*================================================================================================*/
	public class InputForTests : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			UpdateOculus();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void UpdateOculus() {
			if ( Input.GetKey(KeyCode.R) ) {
				InputTracking.Recenter();
			}

			if ( !VRSettings.enabled || !VRDevice.isPresent || !OVRManager.isHSWDisplayed ) {
				return;
			}

			OVRManager.DismissHSWDisplay();
			InputTracking.Recenter();
		}

	}

}
