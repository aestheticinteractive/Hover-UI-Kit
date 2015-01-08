using Leap;
using UnityEngine;

namespace HandMenu.Input {

	/*================================================================================================*/
	public class InputHandData {

		public bool IsLeft { get; set; }
		public float GrabStrength { get; set; }
		public float PalmTowardEyes { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputHandData(Hand pHand) {
			IsLeft = pHand.IsLeft;
			GrabStrength = pHand.GrabStrength;
			PalmTowardEyes = Vector3.Dot(pHand.PalmNormal.ToUnity(), Vector3.down);
		}

	}

}
