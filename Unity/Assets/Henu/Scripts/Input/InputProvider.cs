using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputProvider {

		public Frame Frame { get; private set; }

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
		public void UpdateWithFrame(Frame pFrame) {
			Frame = (pFrame.IsValid ? pFrame : null);
			vInputHandProvL.UpdateWithHand(GetHand(true));
			vInputHandProvR.UpdateWithHand(GetHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public InputHandProvider GetHandProvider(bool pIsLeft) {
			return (pIsLeft ? vInputHandProvL : vInputHandProvR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Hand GetHand(bool pIsLeft) {
			if ( Frame == null ) {
				return null;
			}

			return Frame.Hands.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
