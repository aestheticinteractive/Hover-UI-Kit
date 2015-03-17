using Hover.Common.Items;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastItemState {

		IBaseItem Item { get; }
		float HighlightDistance { get; }
		float HighlightProgress { get; }
		bool IsNearestHighlight { get; }
		bool IsSelectionPrevented { get; }

	}

}
