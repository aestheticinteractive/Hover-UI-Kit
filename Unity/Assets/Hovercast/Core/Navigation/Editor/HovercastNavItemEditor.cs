using UnityEditor;
using UnityEngine;

namespace Hovercast.Core.Navigation.Editor {

	/*================================================================================================*/
	[CustomEditor(typeof(HovercastNavItem))]
	public class HovercastNavItemEditor : UnityEditor.Editor {

		private HovercastNavItem vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HovercastNavItem)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			bool enabled = !Application.isPlaying;

			if ( !enabled ) {
				EditorGUILayout.HelpBox("The following values are for initialization only. To make "+
					"changes at runtime, modify the NavItem objects using scripts.", MessageType.Info);
			}

			GUI.enabled = enabled;

			vTarget.Id = EditorGUILayout.TextField("ID (optional)", vTarget.Id);
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.RelativeSize = EditorGUILayout.FloatField("Relative Size", vTarget.RelativeSize);
			vTarget.IsVisible = EditorGUILayout.Toggle("Visible", vTarget.IsVisible);
			vTarget.IsEnabled = EditorGUILayout.Toggle("Enabled", vTarget.IsEnabled);

			vTarget.Type = (NavItem.ItemType)EditorGUILayout.EnumPopup("Item Type", vTarget.Type);

			if ( vTarget.Type != NavItem.ItemType.Parent ) {
				vTarget.NavigateBackUponSelect = EditorGUILayout.Toggle(
					"Navigate Back Upon Select", vTarget.NavigateBackUponSelect);
			}

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
					vTarget.SliderAllowJump = EditorGUILayout.Toggle("Allow Jump-To-Value", 
						vTarget.SliderAllowJump);
					break;
			}

			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}

			GUI.enabled = true;
		}

	}

}
