using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public interface IInputHand {

		bool IsLeft { get; }
		Vector3 Center { get; }
		Quaternion Rotation { get; }
		float GrabStrength { get; }
		float PalmTowardEyes { get; }

	}

}
