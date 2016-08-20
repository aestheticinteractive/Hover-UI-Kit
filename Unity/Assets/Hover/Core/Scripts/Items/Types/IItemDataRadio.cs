namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	public interface IItemDataRadio : IItemDataSelectable<bool> {

		string DefaultGroupId { get; }
		string GroupId { get; set; }

	}

}
