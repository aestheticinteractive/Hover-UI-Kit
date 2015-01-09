using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputPointProvider {

		public InputPoint Point { get; private set; }

		private readonly Finger.FingerType vFingerType0;
		private readonly Finger.FingerType? vFingerType1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputPointProvider(Finger.FingerType pFingerType) {
			vFingerType0 = pFingerType;
			vFingerType1 = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public InputPointProvider(Finger.FingerType pFingerType0, Finger.FingerType pFingerType1) {
			vFingerType0 = pFingerType0;
			vFingerType1 = pFingerType1;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapHand(Hand pLeapHand) {
			if ( pLeapHand == null ) {
				Point = null;
				return;
			}

			InputPoint point0 = GetPoint(pLeapHand, vFingerType0);
			Point = point0;

			if ( vFingerType1 != null ) {
				InputPoint point1 = GetPoint(pLeapHand, (Finger.FingerType)vFingerType1);
				Point = InputPoint.Lerp(point0, point1, 0.5f);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static InputPoint GetPoint(Hand pLeapHand, Finger.FingerType pType) {
			Finger finger = pLeapHand.Fingers.FingerType(pType).FirstOrDefault(f => f.IsValid);
			return (finger == null ? null : new InputPoint(pLeapHand, finger));
		}

	}

}
