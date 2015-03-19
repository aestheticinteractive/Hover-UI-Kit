using System;
using Hover.Common.Items.Groups;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class ParentItem : SelectableItem<bool>, IParentItem {

		public IItemGroup ChildLevel { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ParentItem(Func<IBaseItem[]> pGetItems) {
			ChildLevel = new ItemGroup(pGetItems);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = true;
			base.Select();
		}

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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateValueOnLevelChange() {
			if ( true ) { //pDirection == ItemGroup.Direction.TowardParent ) {
				Value = false;
			}
		}

	}

}
