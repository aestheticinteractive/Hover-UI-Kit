namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemGroups { 

		event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		string Title { get; }
		object DisplayContainer { get; }
		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }
		IItemGroup[] Groups { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void ReloadActiveGroups();

	}

}
