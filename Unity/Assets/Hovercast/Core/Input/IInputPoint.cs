using UnityEngine;

namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputPoint {

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
