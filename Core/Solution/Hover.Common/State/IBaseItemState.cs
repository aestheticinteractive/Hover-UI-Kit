using Hover.Common.Input;
using Hover.Common.Items;

namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemState {

		IBaseItem Item { get; }

		bool IsSelectionPrevented { get; }
		int DisplaySelectionPreventions { get; }
		float MinHighlightDistance { get; }
		float MaxHighlightProgress { get; }
		bool IsNearestHighlight { get; }
		float SelectionProgress { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightDistance(CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightProgress(CursorType pCursorType);

	}

}
