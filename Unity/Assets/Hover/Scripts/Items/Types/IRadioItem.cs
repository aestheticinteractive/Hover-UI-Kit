namespace Hover.Items.Types {

	/*================================================================================================*/
	public interface IRadioItem : ISelectableItem<bool> {

		string DefaultGroupId { get; }
		string GroupId { get; set; }

	}

}
