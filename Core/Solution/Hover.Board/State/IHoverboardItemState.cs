using Hover.Common.Input;
using Hover.Common.Items;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardItemState {

		IBaseItem Item { get; }
		float MinHighlightDistance { get; }
		float MaxHighlightProgress { get; }
		bool IsNearestHighlight { get; }
		bool IsSelectionPrevented { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightDistance(CursorType pCursorType);

	}

}
