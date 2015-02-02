using UnityEngine;

namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputCursor {

		bool IsLeft { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
