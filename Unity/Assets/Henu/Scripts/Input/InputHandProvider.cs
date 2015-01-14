using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputHandProvider : IInputHandProvider {

		public bool IsLeft { get; private set; }
		public IInputHand Hand { get; private set; }

		public IInputPoint IndexPoint { get; private set; }
		public IInputPoint MiddlePoint { get; private set; }
		public IInputPoint RingPoint { get; private set; }
		public IInputPoint PinkyPoint { get; private set; }


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

			/*if ( Hand == null ) {
				return;
			}

			Debug.Log("HAND\n"+
				Hand.Center.ToString("0.000")+" / "+Hand.Rotation.ToString("0.000")+"\n"+
				IndexPoint.Position.ToString("0.000")+" / "+IndexPoint.Rotation.ToString("0.000")+"\n"+
				MiddlePoint.Position.ToString("0.000")+" / "+
					MiddlePoint.Rotation.ToString("0.000")+"\n"+
				RingPoint.Position.ToString("0.000")+" / "+RingPoint.Rotation.ToString("0.000")+"\n"+
				PinkyPoint.Position.ToString("0.000")+" / "+PinkyPoint.Rotation.ToString("0.000")+"\n"
			);*/
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
