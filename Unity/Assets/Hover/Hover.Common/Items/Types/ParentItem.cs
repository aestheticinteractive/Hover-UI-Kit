using System;
using Hover.Common.Items.Groups;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class ParentItem : SelectableItem, IParentItem {

		public IItemGroup ChildGroup { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ParentItem(Func<IBaseItem[]> pGetItems) {
			ChildGroup = new ItemGroup(pGetItems);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool NavigateBackUponSelect {
			get {
				return false;
			}
			set {
				if ( value ) {
					throw new Exception("Cannot set NavigateBackUponSelect for "+GetType().Name+".");
				}
			}
		}

	}

}
