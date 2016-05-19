using System.Collections.Generic;
using Hover.Common.Utils;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Editor.Input {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(TreeUpdater))]
	public class TreeUpdaterEditor : UnityEditor.Editor {

		private bool vShowUpdatables;
		private bool vShowChildren;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			TreeUpdater targ = (TreeUpdater)target;
			int upsCount = targ.TreeUpdatablesThisFrame.Count;
			int childCount = targ.TreeChildrenThisFrame.Count;
			
			DrawDefaultInspector();
			
			GUI.enabled = false;
			
			EditorGUILayout.ObjectField("Parent", targ.TreeParentThisFrame, typeof(TreeUpdater), true);
			EditorGUILayout.IntField("Depth Level", targ.TreeDepthLevelThisFrame);
			
			vShowUpdatables = EditorGUILayout.Foldout(
				vShowUpdatables, "Updatable Components ("+upsCount+")");
			
			for ( int i = 0 ; vShowUpdatables && i < upsCount ; i++ ) {
				EditorGUILayout.TextField("    Component "+i, 
					targ.gameObject.name+" ("+targ.TreeUpdatablesThisFrame[i].GetType().Name+")");
			}
			
			vShowChildren = EditorGUILayout.Foldout(
				vShowChildren, "Children ("+childCount+")");
			
			for ( int i = 0 ; vShowChildren && i < childCount ; i++ ) {
				EditorGUILayout.ObjectField("    Child "+i, 
					targ.TreeChildrenThisFrame[i], typeof(TreeUpdater), true);
			}
			
			GUI.enabled = true;
		}

	}

}
