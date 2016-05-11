using Hover.Common.Edit.Utils;
using Hover.Common.State;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.State {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverItemCursorActivity))]
	public class HoverItemCursorActivityEditor : Editor {

		private string vIsHighlightOpenKey;
		private GUIStyle vVertStyle;
		
		private HoverItemCursorActivity vTarget;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vIsHighlightOpenKey = "IsHighlightOpen"+target.GetInstanceID();
			vVertStyle = EditorUtil.GetVerticalSectionStyle();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override bool RequiresConstantRepaint() {
			return EditorPrefs.GetBool(vIsHighlightOpenKey);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vTarget = (HoverItemCursorActivity)target;
			
			DrawDefaultInspector();
			DrawHighlightInfo();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawHighlightInfo() {
			bool isHighOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsHighlightOpenKey),
				"Item Highlight Information");
			EditorPrefs.SetBool(vIsHighlightOpenKey, isHighOpen);
			
			if ( !isHighOpen ) {
				return;
			}
			
			EditorGUILayout.BeginVertical(vVertStyle);
			
			if ( !Application.isPlaying ) {
				EditorGUILayout.HelpBox("At runtime, this section displays live information about "+
					"the relationship between the item and each available cursor. You can access this "+
					"information via code.", MessageType.Info);
				EditorGUILayout.EndVertical();
				return;
			}
			
			GUI.enabled = false;
			EditorGUILayout.Toggle("Is Highlight Prevented", vTarget.IsHighlightPrevented);
			EditorGUILayout.Toggle("Is Highlight Prevented (Via Any Display)",
				vTarget.IsHighlightPreventedViaAnyDisplay());
			EditorGUILayout.Toggle("Is Nearest Across All Items (For Any Cursor)",
				vTarget.IsNearestAcrossAllItemsForAnyCursor);
			EditorGUILayout.Slider("Maximum Highlight Progress", vTarget.MaxHighlightProgress, 0, 1);
			GUI.enabled = true;
			
			for ( int i = 0 ; i < vTarget.Highlights.Count ; i++ ) {
				HoverItemCursorActivity.Highlight high = vTarget.Highlights[i];
				EditorGUILayout.Separator();
				EditorGUILayout.LabelField(high.Data.Type+" Cursor", EditorStyles.boldLabel);
				GUI.enabled = false;
				EditorGUILayout.ObjectField("Data", high.Data, high.Data.GetType(), true);
				EditorGUILayout.Vector3Field("Nearest Position", high.NearestWorldPos);
				EditorGUILayout.FloatField("Distance", high.Distance);
				EditorGUILayout.Slider("Progress", high.Progress, 0, 1);
				GUI.enabled = true;
			}
			
			EditorGUILayout.EndVertical();
		}

	}

}
