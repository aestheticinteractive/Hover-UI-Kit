using Hover.Board.Items;
using UnityEditor;
using UnityEngine;

namespace Hover.Board.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverboardItem))]
	public class HoverboardItemEditor : Editor {

		private HoverboardItem vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HoverboardItem)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			bool enabled = !Application.isPlaying;

			if ( !enabled ) {
				EditorGUILayout.HelpBox("The following values are for initialization only. To make "+
					"changes at runtime, modify the Item objects using scripts.", MessageType.Info);
			}

			GUI.enabled = enabled;

			vTarget.Id = EditorGUILayout.TextField("ID (optional)", vTarget.Id);
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.Width = EditorGUILayout.FloatField("Width", vTarget.Width);
			vTarget.Height = EditorGUILayout.FloatField("Height", vTarget.Height);
			vTarget.IsVisible = EditorGUILayout.Toggle("Visible", vTarget.IsVisible);
			vTarget.IsEnabled = EditorGUILayout.Toggle("Enabled", vTarget.IsEnabled);

			vTarget.Type = (HoverboardItem.HoverboardItemType)EditorGUILayout.EnumPopup(
				"Item Type", vTarget.Type);

			switch ( vTarget.Type ) {
				case HoverboardItem.HoverboardItemType.Checkbox:
					vTarget.CheckboxValue = EditorGUILayout.Toggle("Value", vTarget.CheckboxValue);
					break;

				case HoverboardItem.HoverboardItemType.Radio:
					vTarget.RadioValue = EditorGUILayout.Toggle("Value", vTarget.RadioValue);
					break;

				case HoverboardItem.HoverboardItemType.Slider:
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
