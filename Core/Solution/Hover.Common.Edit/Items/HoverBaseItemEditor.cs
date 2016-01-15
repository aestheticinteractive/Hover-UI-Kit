using Hover.Common.Components.Items;
using Hover.Common.Components.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverBaseItem))]
	public abstract class HoverBaseItemEditor : Editor {

		private bool vIsEventOpen;
		private bool vIsReadOnlyOpen;
		private GUIStyle vVertStyle;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverBaseItemEditor() {
		}
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vVertStyle = new GUIStyle();
			vVertStyle.padding = new RectOffset(16, 0, 0, 0);
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
			
			vIsEventOpen = EditorGUILayout.Foldout(vIsEventOpen, "Events");
			
			if ( vIsEventOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				DrawEventItems();
				EditorGUILayout.EndVertical();
			}
			
			vIsReadOnlyOpen = EditorGUILayout.Foldout(vIsReadOnlyOpen, "Read-Only");
			
			if ( vIsReadOnlyOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				DrawHiddenItems();
				EditorGUILayout.EndVertical();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawMainItems() {
			var t = (HoverBaseItem)target;
		
			t.Id = EditorGUILayout.TextField("Id", t.Id);
			t.Label = EditorGUILayout.TextField("Label", t.Label);
			t.Width = EditorGUILayout.Slider("Width", t.Width, 0.01f, 100f);
			t.Height = EditorGUILayout.Slider("Height", t.Height, 0.01f, 100f);
			t.IsEnabled = EditorGUILayout.Toggle("IsEnabled", t.IsEnabled);
			t.IsVisible = EditorGUILayout.Toggle("IsVisible", t.IsVisible);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawEventItems() {
		}
				
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawHiddenItems() {
			var t = (HoverBaseItem)target;
			
			GUI.enabled = false;
			EditorGUILayout.IntField("AutoId", t.Item.AutoId);
			EditorGUILayout.Toggle("IsAncestryEnabled2", t.Item.IsAncestryEnabled);
			EditorGUILayout.Toggle("IsAncestryVisible", t.Item.IsAncestryVisible);
			GUI.enabled = true;
		}

	}

}
