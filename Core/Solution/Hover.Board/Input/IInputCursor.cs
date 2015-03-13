using UnityEngine;

namespace Hover.Board.Input {

	/*================================================================================================*/
	public interface IInputCursor {

		CursorType Type { get; }
		bool IsAvailable { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }
		float Size { get; }

	}

}
