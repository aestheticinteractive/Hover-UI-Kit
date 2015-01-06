using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandData {

		public bool IsLeft { get; set; }
		public float PalmTowardEyes { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HandData(Hand pHand) {
			IsLeft = pHand.IsLeft;
			PalmTowardEyes = Vector3.Dot(pHand.PalmNormal.ToUnity(), Vector3.down);
		}

	}

}
