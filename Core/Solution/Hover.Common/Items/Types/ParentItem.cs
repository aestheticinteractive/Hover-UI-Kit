using System;
using Hover.Common.Items.Groups;

namespace Hover.Common.Items.Types {

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
