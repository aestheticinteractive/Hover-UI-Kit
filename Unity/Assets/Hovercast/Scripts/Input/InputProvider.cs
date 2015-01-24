using System.Linq;
using Leap;
using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public class InputProvider : IInputProvider {

		private Frame vFrame;
		private readonly InputSide vInputHandProvL;
		private readonly InputSide vInputHandProvR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputProvider(Vector3 pPalmDirection) {
			vInputHandProvL = new InputSide(true, pPalmDirection);
			vInputHandProvR = new InputSide(false, pPalmDirection);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapFrame(Frame pLeapFrame) {
			vFrame = (pLeapFrame.IsValid ? pLeapFrame : null);
			vInputHandProvL.UpdateWithLeapHand(GetLeapHand(true));
			vInputHandProvR.UpdateWithLeapHand(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public IInputSide GetSide(bool pIsLeft) {
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
