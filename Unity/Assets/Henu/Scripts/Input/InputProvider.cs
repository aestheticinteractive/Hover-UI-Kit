using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputProvider {

		private Frame vFrame;
		private readonly InputHandProvider vInputHandProvL;
		private readonly InputHandProvider vInputHandProvR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputProvider() {
			vInputHandProvL = new InputHandProvider(true);
			vInputHandProvR = new InputHandProvider(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapFrame(Frame pLeapFrame) {
			vFrame = (pLeapFrame.IsValid ? pLeapFrame : null);
			vInputHandProvL.UpdateWithLeapHand(GetLeapHand(true));
			vInputHandProvR.UpdateWithLeapHand(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public InputHandProvider GetHandProvider(bool pIsLeft) {
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
