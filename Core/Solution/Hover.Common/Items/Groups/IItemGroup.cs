namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemGroup {

		event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		object DisplayContainer { get; }
		bool IsActive { get; set; }
		IBaseItem[] Items { get; }
		ISelectableItem LastSelectedItem { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T[] GetTypedItems<T>() where T : class, IBaseItem;

	}

}
