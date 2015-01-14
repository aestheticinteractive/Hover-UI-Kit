using Henu.Input;
using UnityEngine;

namespace HenuTest.Input {

	/*================================================================================================*/
	public class TestInputProvider : MonoBehaviour, IInputProvider {

		private TestInputHandProvider vInputHandProvL;
		private TestInputHandProvider vInputHandProvR;
		private TestInputHand vInputHandL;
		private TestInputHand vInputHandR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IInputHandProvider GetHandProvider(bool pIsLeft) {
			if ( vInputHandProvL == null ) {
				Init();
			}

			return (pIsLeft ? vInputHandProvL : vInputHandProvR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Init() {
			GameObject leftObj = gameObject.transform.FindChild("LeftHand").gameObject;
			GameObject rightObj = gameObject.transform.FindChild("RightHand").gameObject;

			vInputHandL = leftObj.GetComponent<TestInputHand>();
			vInputHandR = rightObj.GetComponent<TestInputHand>();

			vInputHandL.IsLeft = true;
			vInputHandR.IsLeft = false;

			vInputHandProvL = new TestInputHandProvider(true, vInputHandL);
			vInputHandProvR = new TestInputHandProvider(false, vInputHandR);
		}

	}

}
