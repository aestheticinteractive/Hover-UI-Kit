using System.Collections.Generic;
using Hover.Core.Cursors;
using UnityEditor;
using UnityEngine;

namespace Hover.Editor.Input {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverCursorDataProvider))]
	public class HovercursorDataProviderEditor : UnityEditor.Editor {

		private HoverCursorDataProvider vTarget;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vTarget = (HoverCursorDataProvider)target;
			
			DrawDefaultInspector();
			EditorGUILayout.Separator();
			DrawCursorList("Cursors", vTarget.Cursors);
			
			if ( vTarget.ExcludedCursors.Count == 0 ) {
				return;
			}
			
			EditorGUILayout.Separator();
			EditorGUILayout.HelpBox("One or more duplicate cursor types were found. The following "+
				"cursors have been excluded from the cursor list.", MessageType.Error);
			DrawCursorList("Excluded Cursors", vTarget.ExcludedCursors);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawCursorList(string pLabel, List<ICursorData> pCursors) {
			EditorGUILayout.LabelField(pLabel, EditorStyles.boldLabel);
			GUI.enabled = false;
			
			for ( int i = 0 ; i < pCursors.Count ; i++ ) {
				ICursorData cursor = pCursors[i];
				EditorGUILayout.ObjectField(cursor.Type+"", (Object)cursor, cursor.GetType(), true);
			}
			
			GUI.enabled = true;
		}

	}

}
