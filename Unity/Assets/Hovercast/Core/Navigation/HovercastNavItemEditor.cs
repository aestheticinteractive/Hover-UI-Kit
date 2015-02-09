using UnityEditor;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	[CustomEditor(typeof(HovercastNavItem))]
	public class HovercastNavItemEditor : Editor {

		private HovercastNavItem vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HovercastNavItem)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.RelativeSize = EditorGUILayout.FloatField("Relative Size", vTarget.RelativeSize);
			vTarget.NavigateBackUponSelect= EditorGUILayout.Toggle(
				"Navigate Back Upon Select", vTarget.NavigateBackUponSelect);
			vTarget.Type = (NavItem.ItemType)EditorGUILayout.EnumPopup("Item Type", vTarget.Type);

			switch ( vTarget.Type ) {
				case NavItem.ItemType.Checkbox:
				case NavItem.ItemType.Radio:
					vTarget.ValueBool = EditorGUILayout.Toggle("Value", vTarget.ValueBool);
					break;

				case NavItem.ItemType.Slider:
					vTarget.ValueFloat = EditorGUILayout.Slider("Value", vTarget.ValueFloat, 0, 1);
					break;
			}
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}
		}

	}

}
