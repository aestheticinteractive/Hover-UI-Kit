using Hover.Common.Items.Groups;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public interface IParentItem : ISelectableItem {

		IItemGroup ChildGroup { get; }

	}

}
