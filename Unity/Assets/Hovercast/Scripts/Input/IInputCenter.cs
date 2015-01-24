using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public interface IInputCenter {

		bool IsLeft { get; }
		Vector3 Position { get; }
		Quaternion Rotation { get; }
		float GrabStrength { get; }
		float PalmTowardEyes { get; }

	}

}
