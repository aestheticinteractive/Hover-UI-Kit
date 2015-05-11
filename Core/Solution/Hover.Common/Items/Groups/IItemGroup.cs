namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemGroup {

		event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		string Title { get; }
		object DisplayContainer { get; }
		
		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }
		bool IsAncestryEnabled { get; set; }
		bool IsAncestryVisible { get; set; }

		IBaseItem[] Items { get; }
		ISelectableItem LastSelectedItem { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T[] GetTypedItems<T>() where T : class, IBaseItem;

		/*--------------------------------------------------------------------------------------------*/
		void ReloadActiveItems();

	}

}
