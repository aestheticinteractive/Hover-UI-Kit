using Hover.Common.Input;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardState {

		IHovercursorState Hovercursor { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ProjectionState GetProjection(CursorType pCursorType);

	}

}
