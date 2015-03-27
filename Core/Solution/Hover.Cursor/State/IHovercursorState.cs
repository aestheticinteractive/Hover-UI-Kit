using Hover.Common.Input;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorState {

		HovercursorVisualSettings VisualSettings { get; }
		IHovercursorInput Input { get; }
		Transform CameraTransform { get; }
		CursorType[] ActiveCursorTypes { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ICursorState GetCursorState(CursorType pType);

		/*--------------------------------------------------------------------------------------------*/
		Transform GetCursorTransform(CursorType pType);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddDelegate(IHovercursorDelegate pDelegate);

		/*--------------------------------------------------------------------------------------------*/
		bool RemoveDelegate(IHovercursorDelegate pDelegate);

	}

}
