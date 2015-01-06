using System.Linq;
using Leap;

namespace HandMenu.Input {

	/*================================================================================================*/
	public class PointProvider {

		public PointData Data { get; private set; }

		private readonly Finger.FingerType vFingerType0;
		private readonly Finger.FingerType? vFingerType1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PointProvider(Finger.FingerType pFingerType) {
			vFingerType0 = pFingerType;
			vFingerType1 = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public PointProvider(Finger.FingerType pFingerType0, Finger.FingerType pFingerType1) {
			vFingerType0 = pFingerType0;
			vFingerType1 = pFingerType1;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithHand(Hand pHand) {
			if ( pHand == null ) {
				return;
			}

			PointData point0 = GetPointData(pHand, vFingerType0);
			Data = point0;

			if ( vFingerType1 != null ) {
				PointData point1 = GetPointData(pHand, (Finger.FingerType)vFingerType1);
				Data = PointData.Lerp(point0, point1, 0.5f);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private PointData GetPointData(Hand pHand, Finger.FingerType pType) {
			if ( pHand == null ) {
				return null;
			}

			Finger finger = pHand.Fingers.FingerType(pType).FirstOrDefault(f => f.IsValid);

			if ( finger == null ) {
				return null;
			}

			return new PointData(pHand, finger);
		}

	}

}
