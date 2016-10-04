using UnityEngine;
using UnityEngine.VR;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public class InputForTests : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			if ( Input.GetKey(KeyCode.R) ) {
				InputTracking.Recenter();
			}
		}

	}

}
