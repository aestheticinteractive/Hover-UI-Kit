using Hover.Common.Input;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardState {

		IHovercursorState Hovercursor { get; }
		IHoverboardPanelState[] Panels { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ProjectionState GetProjection(CursorType pCursorType);

	}

}
