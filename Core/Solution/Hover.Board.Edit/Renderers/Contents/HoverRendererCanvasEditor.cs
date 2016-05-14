using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Helpers;
using UnityEditor;
using UnityEngine;

namespace Hover.Board.Edit.Renderers {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverRendererCanvas))]
	public class HoverRendererCanvasEditor  : Editor {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			HoverRendererCanvas targ = (HoverRendererCanvas)target;
			bool parentCont = false; //RendererHelper.IsUpdatePreventedBy(targ.ParentController);
			bool parentRend = RendererHelper.IsUpdatePreventedBy(targ.ParentRenderer);

			if ( parentCont || parentRend ) {
				EditorGUILayout.HelpBox(RendererHelper.GetDisabledSettingsText(targ.ParentRenderer), 
					MessageType.Warning);
			}

			Undo.RecordObject(targ, targ.GetType().Name);

			serializedObject.Update();
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Label"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("IconOuter"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("IconInner"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Scale"));
			GUI.enabled = !parentRend;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeX"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeY"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("PaddingX"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("PaddingY"));
			GUI.enabled = !parentRend;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Alpha"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Alignment"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("IconSize"));
			GUI.enabled = !parentRend;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RenderQueue"));
			serializedObject.ApplyModifiedProperties();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(targ);
			}
		}
		
	}

}
