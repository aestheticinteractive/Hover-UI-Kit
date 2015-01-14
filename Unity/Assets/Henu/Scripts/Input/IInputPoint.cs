using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public interface IInputPoint {

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
