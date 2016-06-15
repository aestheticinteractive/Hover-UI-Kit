using System;
using Hover.Items.Groups;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataParent : SelectableItem, IParentItem {

		public IItemGroup ChildGroup { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void InitChildGroup(Func<IBaseItem[]> pGetItems) {
			ChildGroup = new ItemGroup(pGetItems);
		}

	}

}
