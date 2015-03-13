using Hover.Cast.Navigation;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastItemState {

		NavItem NavItem { get; }
		float HighlightDistance { get; }
		float HighlightProgress { get; }
		bool IsNearestHighlight { get; }
		bool IsSelectionPrevented { get; }

	}

}
