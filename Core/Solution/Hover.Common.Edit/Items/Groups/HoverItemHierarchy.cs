using Hover.Common.Items.Groups;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items.Groups {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverItemHierarchy))]
	public class HoverItemHierarchyEditor : Editor {

		protected GUIStyle vVertStyle;

		private SerializedProperty vOnLevelChangedProp;
		private SerializedProperty vOnItemSelectedProp;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnEnable() {
			vVertStyle = new GUIStyle();
			vVertStyle.padding = new RectOffset(16, 0, 0, 0);
			
			var t = (HoverItemHierarchy)target;
			string onLvlChangedName = HoverBaseItemEditor.GetPropertyName(() => t.OnLevelChangedEvent);
			string onItemSelName = HoverBaseItemEditor.GetPropertyName(() => t.OnItemSelectedEvent);
			
			vOnLevelChangedProp = serializedObject.FindProperty(onLvlChangedName);
			vOnItemSelectedProp = serializedObject.FindProperty(onItemSelName);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(target, target.GetType().Name);
			
			DrawItems();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(target);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawItems() {
			DrawMainItems();
			DrawEventItems();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawMainItems() {
			HoverItemHierarchy t = (HoverItemHierarchy)target;
		
			t.Title = EditorGUILayout.TextField("Title", t.Title);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawEventItems() {
			EditorGUILayout.PropertyField(vOnLevelChangedProp);
			EditorGUILayout.PropertyField(vOnItemSelectedProp);
		}

	}

}
