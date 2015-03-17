using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorState {

		HovercursorCustomizationProvider CustomizationProvider { get; }
		HovercursorInputProvider InputProvider { get; }

		Transform CameraTransform { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ICursorState GetCursorState(CursorType pType);

		/*--------------------------------------------------------------------------------------------*/
		Transform GetCursorTransform(CursorType pType);

	}

}
