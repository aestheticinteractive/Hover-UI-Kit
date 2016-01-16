using Hover.Common.Components.Items.Types;
using UnityEditor;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSelectorItem))]
	public class HoverSelectorItemEditor : HoverSelectableItemEditor {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSelectorItem)target;
			
			t.NavigateBackUponSelect = EditorGUILayout.Toggle(
				"Navigate Back Upon Select", t.NavigateBackUponSelect);
		}
		
	}

}
