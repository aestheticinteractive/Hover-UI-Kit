using Hover.Board.Input;
using Hover.Board.Navigation;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardItemState {

		NavItem NavItem { get; }
		float MinHighlightDistance { get; }
		float MaxHighlightProgress { get; }
		bool IsNearestHighlight { get; }
		bool IsSelectionPrevented { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightDistance(CursorType pCursorType);

	}

}
