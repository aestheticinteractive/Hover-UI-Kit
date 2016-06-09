using Hover.Items.Groups;

namespace Hover.Items.Types {

	/*================================================================================================*/
	public interface IParentItem : ISelectableItem {

		IItemGroup ChildGroup { get; }

	}

}
