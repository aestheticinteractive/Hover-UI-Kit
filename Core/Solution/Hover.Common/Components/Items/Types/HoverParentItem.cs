using Hover.Common.Items.Groups;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverParentItem : HoverSelectableItem, IParentItem {

		public IItemGroup ChildGroup { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverParentItem() {
			ChildGroup = new ItemGroup(() => GetChildItems(gameObject));
		}

	}

}
