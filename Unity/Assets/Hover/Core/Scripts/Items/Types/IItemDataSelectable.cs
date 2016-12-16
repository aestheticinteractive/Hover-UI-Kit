namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	public interface IItemDataSelectable : IItemData {

		event ItemEvents.SelectedHandler OnSelected;
		event ItemEvents.DeselectedHandler OnDeselected;

		bool IsStickySelected { get; }
		bool IgnoreSelection { get; }
		bool AllowIdleDeselection { get; set;  }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Select();

		/*--------------------------------------------------------------------------------------------*/
		void DeselectStickySelections();

	}

}
