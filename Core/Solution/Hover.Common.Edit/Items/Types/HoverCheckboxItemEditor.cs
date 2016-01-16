using Hover.Common.Components.Items.Types;
using UnityEditor;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverCheckboxItem))]
	public class HoverCheckboxtemEditor : HoverSelectableItemBoolEditor {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			vValueLabel = "Checkbox Value";
		}
		
	}

}
