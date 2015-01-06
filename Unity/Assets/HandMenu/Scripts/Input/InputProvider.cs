using System.Linq;
using Leap;

namespace HandMenu.Input {

	/*================================================================================================*/
	public class InputProvider {

		public Frame Frame { get; private set; }

		private readonly HandProvider vHandProvL;
		private readonly HandProvider vHandProvR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputProvider() {
			vHandProvL = new HandProvider(true);
			vHandProvR = new HandProvider(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithFrame(Frame pFrame) {
			Frame = (pFrame.IsValid ? pFrame : null);
			vHandProvL.UpdateWithHand(GetHand(true));
			vHandProvR.UpdateWithHand(GetHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public HandProvider GetHandProvider(bool pIsLeft) {
			return (pIsLeft ? vHandProvL : vHandProvR);
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
