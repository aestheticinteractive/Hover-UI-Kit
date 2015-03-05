using Hoverboard.Core.Navigation;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public interface IHovercastItemState {

		NavItem NavItem { get; }
		float MinHighlightDistance { get; }
		float MaxHighlightProgress { get; }
		bool IsNearestHighlight { get; }
		bool IsSelectionPrevented { get; }

	}

}
