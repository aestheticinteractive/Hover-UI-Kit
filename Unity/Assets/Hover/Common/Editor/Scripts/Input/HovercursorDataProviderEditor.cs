using System.Collections.Generic;
using Hover.Common.Input;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Editor.Input {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HovercursorDataProvider))]
	public class HovercursorDataProviderEditor : UnityEditor.Editor {

		private HovercursorDataProvider vTarget;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vTarget = (HovercursorDataProvider)target;
			
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
		private void DrawCursorList(string pLabel, List<HovercursorData> pCursors) {
			EditorGUILayout.LabelField(pLabel, EditorStyles.boldLabel);
			GUI.enabled = false;
			
			for ( int i = 0 ; i < pCursors.Count ; i++ ) {
				HovercursorData cursor = pCursors[i];
				EditorGUILayout.ObjectField(cursor.Type+"", cursor, cursor.GetType(), true);
			}
			
			GUI.enabled = true;
		}

	}

}
