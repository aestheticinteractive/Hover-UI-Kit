using Hover.Common.Components.Items.Types;
using UnityEditor;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverParentItem))]
	public class HoverParentEditor : HoverSelectableItemEditor {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			vHideNavBack = true;
		}
		
	}

}
