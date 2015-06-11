using System.Collections.ObjectModel;
using Hover.Common.Input;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardState {

		IHovercursorState Hovercursor { get; }
		ReadOnlyCollection<IHoverboardPanelState> Panels { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ProjectionState GetProjection(CursorType pCursorType);

	}

}
