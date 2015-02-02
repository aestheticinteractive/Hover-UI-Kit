using UnityEngine;

namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputCursor {

		bool IsLeft { get; }
		bool IsActive { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }

	}

}
