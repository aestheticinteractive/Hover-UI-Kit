using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public interface IInputPoint {

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
