using Hovercast.Core;
using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class HovercastTestInputProvider : HovercastInputProvider {

		//TODO: update testing input

		private TestInputSide vInputHandProvL;
		private TestInputSide vInputHandProvR;
		private TestInputMenu vInputMenuL;
		private TestInputMenu vInputMenuR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput(bool pIsMenuOnLeftSide) {
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

			vInputMenuL = leftObj.GetComponent<TestInputMenu>();
			vInputMenuR = rightObj.GetComponent<TestInputMenu>();

			vInputMenuL.IsLeft = true;
			vInputMenuR.IsLeft = false;

			vInputHandProvL = new TestInputSide(true, vInputMenuL);
			vInputHandProvR = new TestInputSide(false, vInputMenuR);
		}

	}

}
