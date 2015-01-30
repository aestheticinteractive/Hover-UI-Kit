using Hovercast.Core;
using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class HovercastTestInputProvider : HovercastInputProvider {

		private TestInputSide vInputHandProvL;
		private TestInputSide vInputHandProvR;
		private TestInputCenter vInputCenterL;
		private TestInputCenter vInputCenterR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override IInputSide GetSide(bool pIsLeft) {
			if ( vInputHandProvL == null ) {
				Init();
			}

			return (pIsLeft ? vInputHandProvL : vInputHandProvR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Init() {
			PalmDirection = Vector3.down;

			GameObject leftObj = gameObject.transform.FindChild("LeftHand").gameObject;
			GameObject rightObj = gameObject.transform.FindChild("RightHand").gameObject;

			vInputCenterL = leftObj.GetComponent<TestInputCenter>();
			vInputCenterR = rightObj.GetComponent<TestInputCenter>();

			vInputCenterL.IsLeft = true;
			vInputCenterR.IsLeft = false;

			vInputHandProvL = new TestInputSide(true, vInputCenterL);
			vInputHandProvR = new TestInputSide(false, vInputCenterR);
		}

	}

}
