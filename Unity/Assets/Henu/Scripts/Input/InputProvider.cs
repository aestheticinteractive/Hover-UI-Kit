using System.Linq;
using Leap;
using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputProvider : IInputProvider {

		private Frame vFrame;
		private readonly InputHandProvider vInputHandProvL;
		private readonly InputHandProvider vInputHandProvR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputProvider(Vector3 pPalmDirection) {
			vInputHandProvL = new InputHandProvider(true, pPalmDirection);
			vInputHandProvR = new InputHandProvider(false, pPalmDirection);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapFrame(Frame pLeapFrame) {
			vFrame = (pLeapFrame.IsValid ? pLeapFrame : null);
			vInputHandProvL.UpdateWithLeapHand(GetLeapHand(true));
			vInputHandProvR.UpdateWithLeapHand(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public IInputHandProvider GetHandProvider(bool pIsLeft) {
			return (pIsLeft ? vInputHandProvL : vInputHandProvR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Hand GetLeapHand(bool pIsLeft) {
			if ( vFrame == null ) {
				return null;
			}

			return vFrame.Hands.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
