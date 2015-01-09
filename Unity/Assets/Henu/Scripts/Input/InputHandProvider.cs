using System.Collections.Generic;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputHandProvider {

		public bool IsLeft { get; private set; }
		public InputHand Hand { get; private set; }

		private readonly IDictionary<InputPointData.PointZone, InputPointProvider> vPointProvMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputHandProvider(bool pIsLeft) {
			IsLeft = pIsLeft;

			vPointProvMap = new Dictionary<InputPointData.PointZone, InputPointProvider> {
				{
					InputPointData.PointZone.Index, 
					new InputPointProvider(Finger.FingerType.TYPE_INDEX)
				},
				{
					InputPointData.PointZone.IndexMiddle,
					new InputPointProvider(Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE)
				},
				{
					InputPointData.PointZone.Middle, 
					new InputPointProvider(Finger.FingerType.TYPE_MIDDLE)
				},
				{
					InputPointData.PointZone.MiddleRing,
					new InputPointProvider(Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING)
				},
				{
					InputPointData.PointZone.Ring,
					new InputPointProvider(Finger.FingerType.TYPE_RING)
				},
				{
					InputPointData.PointZone.RingPinky,
					new InputPointProvider(Finger.FingerType.TYPE_RING, Finger.FingerType.TYPE_PINKY)
				},
				{
					InputPointData.PointZone.Pinky,
					new InputPointProvider(Finger.FingerType.TYPE_PINKY)
				}
			};
		}
		


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithHand(Hand pLeapHand) {
			Hand = (pLeapHand == null ? null : new InputHand(pLeapHand));

			foreach ( InputPointProvider pointProv in vPointProvMap.Values ) {
				pointProv.UpdateWithHand(pLeapHand);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public InputPointProvider GetPointProvider(InputPointData.PointZone pZone) {
			return vPointProvMap[pZone];
		}

	}

}
