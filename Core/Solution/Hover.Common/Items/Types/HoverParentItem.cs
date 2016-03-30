using Hover.Common.Items.Groups;

namespace Hover.Common.Items.Types {

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
