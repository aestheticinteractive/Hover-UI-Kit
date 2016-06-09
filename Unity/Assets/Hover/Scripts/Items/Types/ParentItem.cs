using System;
using Hover.Items.Groups;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class ParentItem : SelectableItem, IParentItem {

		public IItemGroup ChildGroup { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void InitChildGroup(Func<IBaseItem[]> pGetItems) {
			ChildGroup = new ItemGroup(pGetItems);
		}

	}

}
