using Hover.Core.Items.Managers;
using UnityEditor;
using UnityEngine;

namespace Hover.Editor.Items.Managers {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverItemSelectionState))]
	public class HoverItemSelectionStateEditor : UnityEditor.Editor {

		private string vIsSelectionOpenKey;
		private GUIStyle vVertStyle;
		
		private HoverItemSelectionState vTarget;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vIsSelectionOpenKey = "IsSelectionOpen"+target.GetInstanceID();
			vVertStyle = EditorUtil.GetVerticalSectionStyle();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override bool RequiresConstantRepaint() {
			return EditorPrefs.GetBool(vIsSelectionOpenKey);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vTarget = (HoverItemSelectionState)target;
			
			DrawDefaultInspector();
			DrawSelectionInfo();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawSelectionInfo() {
			bool isHighOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsSelectionOpenKey),
				"Item Selection Information");
			EditorPrefs.SetBool(vIsSelectionOpenKey, isHighOpen);
			
			if ( !isHighOpen ) {
				return;
			}
			
			EditorGUILayout.BeginVertical(vVertStyle);
			
			if ( !Application.isPlaying ) {
				EditorGUILayout.HelpBox("At runtime, this section displays live information about "+
					"the item's selection state. You can access this "+
					"information via code.", MessageType.Info);
				EditorGUILayout.EndVertical();
				return;
			}
			
			GUI.enabled = false;
			EditorGUILayout.Toggle("Is Selection Prevented", vTarget.IsSelectionPrevented);
			EditorGUILayout.Slider("Selection Progress", vTarget.SelectionProgress, 0, 1);
			GUI.enabled = true;
			
			EditorGUILayout.EndVertical();
		}

	}

}
