using Hover.Common.Items.Types;
using UnityEditor;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverRadioItem))]
	public class HoverRadioItemEditor : HoverSelectableItemBoolEditor {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			vValueLabel = "Radio Value";
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverRadioItem)target;
			
			t.GroupId = EditorGUILayout.TextField("Radio Group ID", t.GroupId);
		}
		
	}

}
