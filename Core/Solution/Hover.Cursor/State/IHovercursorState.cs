using Hover.Common.Input;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorState {

		HovercursorVisualSettings CustomizationProvider { get; }
		HovercursorInputProvider InputProvider { get; }
		CursorType[] InitializedCursorTypes { get; }
		Transform CameraTransform { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ICursorState GetCursorState(CursorType pType);

		/*--------------------------------------------------------------------------------------------*/
		Transform GetCursorTransform(CursorType pType);

	}

}
