namespace Hover.Items.Types {

	/*================================================================================================*/
	public interface IItemDataSelectable : IItemData {

		event ItemEvents.SelectedHandler OnSelected;
		event ItemEvents.DeselectedHandler OnDeselected;

		bool IsStickySelected { get; }
		bool AllowSelection { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Select();

		/*--------------------------------------------------------------------------------------------*/
		void DeselectStickySelections();

	}

}
