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
		private static void UpdateOculus() {
			if ( Input.GetKey(KeyCode.R) ) {
				InputTracking.Recenter();
			}

			if ( !OVRManager.isHSWDisplayed ) {
				return;
			}

			OVRManager.DismissHSWDisplay();
			InputTracking.Recenter();
		}

	}

}
