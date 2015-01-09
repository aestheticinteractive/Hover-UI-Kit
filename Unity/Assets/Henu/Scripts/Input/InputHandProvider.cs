using System.Collections.Generic;
using Leap;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputHandProvider {

		public bool IsLeft { get; private set; }
		public InputHand Hand { get; private set; }

		private readonly IDictionary<InputPointZone, InputPointProvider> vPointProvMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputHandProvider(bool pIsLeft) {
			IsLeft = pIsLeft;

			vPointProvMap = new Dictionary<InputPointZone, InputPointProvider> {
				{
					InputPointZone.Index, 
					new InputPointProvider(Finger.FingerType.TYPE_INDEX)
				},
				{
					InputPointZone.IndexMiddle,
					new InputPointProvider(Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE)
				},
				{
					InputPointZone.Middle, 
					new InputPointProvider(Finger.FingerType.TYPE_MIDDLE)
				},
				{
					InputPointZone.MiddleRing,
					new InputPointProvider(Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING)
				},
				{
					InputPointZone.Ring,
					new InputPointProvider(Finger.FingerType.TYPE_RING)
				},
				{
					InputPointZone.RingPinky,
					new InputPointProvider(Finger.FingerType.TYPE_RING, Finger.FingerType.TYPE_PINKY)
				},
				{
					InputPointZone.Pinky,
					new InputPointProvider(Finger.FingerType.TYPE_PINKY)
				}
			};
		}
		


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapHand(Hand pLeapHand) {
			Hand = (pLeapHand == null ? null : new InputHand(pLeapHand));

			foreach ( InputPointProvider pointProv in vPointProvMap.Values ) {
				pointProv.UpdateWithLeapHand(pLeapHand);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public InputPointProvider GetPointProvider(InputPointZone pZone) {
			return vPointProvMap[pZone];
		}

	}

}
