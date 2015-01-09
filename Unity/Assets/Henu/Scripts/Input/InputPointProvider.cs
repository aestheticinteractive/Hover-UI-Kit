using System.Linq;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputPointProvider {

		public InputPointData Data { get; private set; }

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
		public void UpdateWithHand(Hand pHand) {
			if ( pHand == null ) {
				Data = null;
				return;
			}

			InputPointData point0 = GetPointData(pHand, vFingerType0);
			Data = point0;

			if ( vFingerType1 != null ) {
				InputPointData point1 = GetPointData(pHand, (Finger.FingerType)vFingerType1);
				Data = InputPointData.Lerp(point0, point1, 0.5f);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static InputPointData GetPointData(Hand pHand, Finger.FingerType pType) {
			Finger finger = pHand.Fingers.FingerType(pType).FirstOrDefault(f => f.IsValid);
			return (finger == null ? null : new InputPointData(pHand, finger));
		}

	}

}
