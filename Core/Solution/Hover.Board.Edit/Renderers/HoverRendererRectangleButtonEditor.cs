using Hover.Board.Renderers;
using Hover.Board.Renderers.Helpers;
using UnityEditor;
using UnityEngine;

namespace Hover.Board.Edit.Renderers {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverRendererRectangleButton))]
	public class HoverRendererRectangleButtonEditor : Editor {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			HoverRendererRectangleButton targ = (HoverRendererRectangleButton)target;
			bool parentCont = RendererHelper.IsUpdatePreventedBy(targ.ParentController);
			bool parentRend = RendererHelper.IsUpdatePreventedBy(targ.ParentRenderer);

			if ( parentCont || parentRend ) {
				EditorGUILayout.HelpBox(RendererHelper.GetDisabledSettingsText(
					targ.ParentController ?? targ.ParentRenderer), MessageType.Warning);
			}

			Undo.RecordObject(targ, targ.GetType().Name);

			serializedObject.Update();
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Fill"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Canvas"));
			GUI.enabled = (!parentCont && !parentRend);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeX"));
			GUI.enabled = !parentCont;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeY"));
			GUI.enabled = (!parentCont && !parentRend);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Alpha"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("Anchor"));
			serializedObject.ApplyModifiedProperties();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(targ);
			}
		}
		
	}

}
