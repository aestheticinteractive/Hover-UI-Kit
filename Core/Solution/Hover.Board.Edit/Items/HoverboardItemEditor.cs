using Hover.Board.Items;
using Hover.Common.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Board.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverboardItem))]
	public class HoverboardItemEditor : Editor {

		private HoverboardItem vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HoverboardItem)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			bool enabled = !Application.isPlaying;

			if ( !enabled ) {
				EditorGUILayout.HelpBox("The following values are for initialization only. To make "+
					"changes at runtime, modify the Item objects using scripts.", MessageType.Info);
			}

			GUI.enabled = enabled;

			vTarget.Id = EditorGUILayout.TextField("ID (optional)", vTarget.Id);
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.Width = EditorGUILayout.IntField("Width", vTarget.Width);
			vTarget.Height = EditorGUILayout.IntField("Height", vTarget.Height);
			vTarget.IsVisible = EditorGUILayout.Toggle("Visible", vTarget.IsVisible);
			vTarget.IsEnabled = EditorGUILayout.Toggle("Enabled", vTarget.IsEnabled);

			//vTarget.Type = (SelectableItemType)EditorGUILayout.EnumPopup("Item Type", vTarget.Type);
			vTarget.Type = SelectableItemType.Selector;

			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}

			GUI.enabled = true;
		}

	}

}
