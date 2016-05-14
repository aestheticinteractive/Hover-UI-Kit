using Hover.Board.Renderers;
using Hover.Board.Renderers.Helpers;
using UnityEditor;
using UnityEngine;

namespace Hover.Board.Edit.Renderers {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverRendererRectangleSlider))]
	public class HoverRendererRectangleSliderEditor : Editor {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			HoverRendererRectangleSlider targ = (HoverRendererRectangleSlider)target;
			bool parentCont = RendererHelper.IsUpdatePreventedBy(targ.ParentController);

			if ( parentCont ) {
				EditorGUILayout.HelpBox(RendererHelper.GetDisabledSettingsText(targ.ParentController), 
					MessageType.Warning);
			}

			Undo.RecordObject(targ, targ.GetType().Name);

			serializedObject.Update();
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Container"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Track"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("HandleButton"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("JumpButton"));
			GUI.enabled = !parentCont;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeX"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeY"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Alpha"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ZeroValue"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("HandleValue"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("JumpValue"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowJump"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("FillStartingPoint"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Anchor"));
			serializedObject.ApplyModifiedProperties();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(targ);
			}
		}
		
	}

}
