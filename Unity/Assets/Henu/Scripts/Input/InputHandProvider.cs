using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputHandProvider {

		public bool IsLeft { get; private set; }
		public InputHand Hand { get; private set; }

		public InputPoint IndexPoint { get; private set; }
		public InputPoint MiddlePoint { get; private set; }
		public InputPoint RingPoint { get; private set; }
		public InputPoint PinkyPoint { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputHandProvider(bool pIsLeft) {
			IsLeft = pIsLeft;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapHand(Hand pLeapHand) {
			Hand = (pLeapHand == null ? null : new InputHand(pLeapHand));

			IndexPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_INDEX);
			MiddlePoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_MIDDLE);
			RingPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_RING);
			PinkyPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_PINKY);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static InputPoint GetPoint(Hand pLeapHand, Finger.FingerType pFingerType) {
			if ( pLeapHand == null ) {
				return null;
			}

			Finger leapFinger = pLeapHand.Fingers
				.FingerType(pFingerType)
				.FirstOrDefault(f => f.IsValid);

			if ( leapFinger == null ) {
				return null;
			}

			return new InputPoint(leapFinger);
		}

	}

}
