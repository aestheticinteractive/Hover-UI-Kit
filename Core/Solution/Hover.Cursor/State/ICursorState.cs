using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface ICursorState {

		CursorType Type { get; }
		bool IsInputAvailable { get; }
		Vector3 Position { get; }
		float Size { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetWorldPosition();

	}

}
