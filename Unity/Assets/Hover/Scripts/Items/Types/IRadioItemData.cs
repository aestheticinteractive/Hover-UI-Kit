namespace Hover.Items.Types {

	/*================================================================================================*/
	public interface IRadioItemData : ISelectableItemData<bool> {

		string DefaultGroupId { get; }
		string GroupId { get; set; }

	}

}
