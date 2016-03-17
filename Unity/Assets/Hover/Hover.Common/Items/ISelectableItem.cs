namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface ISelectableItem : IBaseItem {

		event ItemEvents.SelectedHandler OnSelected;
		event ItemEvents.DeselectedHandler OnDeselected;

		bool IsStickySelected { get; }
		bool AllowSelection { get; }
		bool NavigateBackUponSelect { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Select();

		/*--------------------------------------------------------------------------------------------*/
		void DeselectStickySelections();

	}

}
