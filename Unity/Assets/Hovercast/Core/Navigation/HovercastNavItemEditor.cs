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

			vTarget.Id = EditorGUILayout.TextField("ID (optional)", vTarget.Id);
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.RelativeSize = EditorGUILayout.FloatField("Relative Size", vTarget.RelativeSize);
			vTarget.IsEnabled = EditorGUILayout.Toggle("Enabled", vTarget.IsEnabled);
			vTarget.NavigateBackUponSelect = EditorGUILayout.Toggle(
				"Navigate Back Upon Select", vTarget.NavigateBackUponSelect);

			vTarget.Type = (NavItem.ItemType)EditorGUILayout.EnumPopup("Item Type", vTarget.Type);

			switch ( vTarget.Type ) {
				case NavItem.ItemType.Checkbox:
					vTarget.CheckboxValue = EditorGUILayout.Toggle("Value", vTarget.CheckboxValue);
					break;

				case NavItem.ItemType.Radio:
					vTarget.RadioValue = EditorGUILayout.Toggle("Value", vTarget.RadioValue);
					break;

				case NavItem.ItemType.Slider:
					vTarget.SliderTicks = EditorGUILayout.IntField("Ticks", vTarget.SliderTicks);
					vTarget.SliderSnaps = EditorGUILayout.IntField("Snaps", vTarget.SliderSnaps);
					vTarget.SliderRangeMin = EditorGUILayout.FloatField("Min", vTarget.SliderRangeMin);
					vTarget.SliderRangeMax = EditorGUILayout.FloatField("Max", vTarget.SliderRangeMax);
					vTarget.SliderValue = EditorGUILayout.Slider("Value", vTarget.SliderValue,	
						vTarget.SliderRangeMin, vTarget.SliderRangeMax);
					break;
			}
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}
		}

	}

}
