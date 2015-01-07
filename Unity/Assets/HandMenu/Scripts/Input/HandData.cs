using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandData {

		public bool IsLeft { get; set; }
		public float GrabStrength { get; set; }
		public float PalmTowardEyes { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HandData(Hand pHand) {
			IsLeft = pHand.IsLeft;
			GrabStrength = pHand.GrabStrength;
			PalmTowardEyes = Vector3.Dot(pHand.PalmNormal.ToUnity(), Vector3.down);
		}

	}

}
