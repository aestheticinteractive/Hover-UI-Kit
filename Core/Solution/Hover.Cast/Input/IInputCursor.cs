using UnityEngine;

namespace Hover.Cast.Input {

	/*================================================================================================*/
	public interface IInputCursor {

		bool IsLeft { get; }
		bool IsAvailable { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
