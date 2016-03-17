namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemInteractionState {

		int ItemAutoId { get; }
		bool IsSelectionPrevented { get; }
		float MaxHighlightProgress { get; }
		float SelectionProgress { get; }

	}

}
