using Hover.Common.Renderers;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Editor.Renderers {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverRendererRectangleController))]
	public class HoverRendererRectangleControllerEditor : UnityEditor.Editor {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			HoverRendererRectangleController targ = (HoverRendererRectangleController)target;

			Undo.RecordObject(targ, targ.GetType().Name);
			
			serializedObject.Update();
			GUI.enabled = targ.IsButtonRendererType;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ButtonRenderer"));
			GUI.enabled = !targ.IsButtonRendererType;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SliderRenderer"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeX"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeY"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("DisabledAlpha"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowProximityDebugLines"));
			serializedObject.ApplyModifiedProperties();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(targ);
			}
		}
		
	}

}
