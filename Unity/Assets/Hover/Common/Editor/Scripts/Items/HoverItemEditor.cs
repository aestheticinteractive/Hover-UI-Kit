using Hover.Common.Items;
using UnityEditor;

namespace Hover.Common.Editor.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverItem))]
	[CanEditMultipleObjects]
	public class HoverItemEditor : UnityEditor.Editor {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			serializedObject.Update();
			
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_ItemType"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Data"));

			serializedObject.ApplyModifiedProperties();
			((HoverItem)target).UpdateAfterInspector();
		}

	}

}
