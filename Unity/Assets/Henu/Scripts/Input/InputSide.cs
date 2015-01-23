using System.Linq;
using Leap;
using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputSide : IInputSide {

		public bool IsLeft { get; private set; }
		public IInputCenter Center { get; private set; }

		public IInputPoint[] Points { get; private set; }
		public IInputPoint IndexPoint { get; private set; }
		public IInputPoint MiddlePoint { get; private set; }
		public IInputPoint RingPoint { get; private set; }
		public IInputPoint PinkyPoint { get; private set; }

		private readonly IInputPoint[] vPoints;
		private readonly Vector3 vPalmDirection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputSide(bool pIsLeft, Vector3 pPalmDirection) {
			IsLeft = pIsLeft;
			vPalmDirection = pPalmDirection;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapHand(Hand pLeapHand) {
			Center = (pLeapHand == null ? null : new InputCenter(pLeapHand, vPalmDirection));

			IndexPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_INDEX);
			MiddlePoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_MIDDLE);
			RingPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_RING);
			PinkyPoint = GetPoint(pLeapHand, Finger.FingerType.TYPE_PINKY);

			Points = new [] { IndexPoint, MiddlePoint, RingPoint, PinkyPoint };

			/*if ( Center == null ) {
				return;
			}

			Debug.Log("HAND\n"+
				Center.Position.ToString("0.000")+" / "+Center.Rotation.ToString("0.000")+"\n"+
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
