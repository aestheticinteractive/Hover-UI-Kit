namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemGroups { 

		event ItemEvents.GroupItemSelectedHandler OnItemSelection;

		string Title { get; }
		object DisplayContainer { get; }
		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }
		IItemGroup[] Groups { get; }

	}

}
