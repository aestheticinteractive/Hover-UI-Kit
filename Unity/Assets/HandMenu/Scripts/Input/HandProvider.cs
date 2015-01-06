using System.Collections.Generic;
using Leap;

namespace HandMenu.Input {

	/*================================================================================================*/
	public class HandProvider {

		public bool IsLeft { get; private set; }
		public HandData Data { get; private set; }

		private readonly IDictionary<PointData.PointZone, PointProvider> vPointProvMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HandProvider(bool pIsLeft) {
			IsLeft = pIsLeft;

			vPointProvMap = new Dictionary<PointData.PointZone, PointProvider> {
				{
					PointData.PointZone.Index, 
					new PointProvider(Finger.FingerType.TYPE_INDEX)
				},
				{
					PointData.PointZone.IndexMiddle,
					new PointProvider(Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE)
				},
				{
					PointData.PointZone.Middle, 
					new PointProvider(Finger.FingerType.TYPE_MIDDLE)
				},
				{
					PointData.PointZone.MiddleRing,
					new PointProvider(Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING)
				},
				{
					PointData.PointZone.Ring,
					new PointProvider(Finger.FingerType.TYPE_RING)
				},
				{
					PointData.PointZone.RingPinky,
					new PointProvider(Finger.FingerType.TYPE_RING, Finger.FingerType.TYPE_PINKY)
				},
				{
					PointData.PointZone.Pinky,
					new PointProvider(Finger.FingerType.TYPE_PINKY)
				}
			};
		}
		


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithHand(Hand pHand) {
			Data = (pHand == null ? null : new HandData(pHand));

			foreach ( PointProvider pointProv in vPointProvMap.Values ) {
				pointProv.UpdateWithHand(pHand);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public PointProvider GetPointProvider(PointData.PointZone pZone) {
			return vPointProvMap[pZone];
		}

	}

}
