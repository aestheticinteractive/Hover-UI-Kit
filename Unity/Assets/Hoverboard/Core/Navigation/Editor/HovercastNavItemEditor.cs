using UnityEditor;
using UnityEngine;

namespace Hoverboard.Core.Navigation.Editor {

	/*================================================================================================*/
	[CustomEditor(typeof(HovercastNavItem))]
	public class HovercastNavItemEditor : UnityEditor.Editor {

		private HovercastNavItem vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HovercastNavItem)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			bool enabled = !Application.isPlaying;

			if ( !enabled ) {
				EditorGUILayout.HelpBox("The following values are for initialization only. To make "+
					"changes at runtime, modify the NavItem objects using scripts.", MessageType.Info);
			}

			GUI.enabled = enabled;

			vTarget.Id = EditorGUILayout.TextField("ID (optional)", vTarget.Id);
			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.Width = EditorGUILayout.IntField("Width", vTarget.Width);
			vTarget.IsVisible = EditorGUILayout.Toggle("Visible", vTarget.IsVisible);
			vTarget.IsEnabled = EditorGUILayout.Toggle("Enabled", vTarget.IsEnabled);

			vTarget.Type = (NavItem.ItemType)EditorGUILayout.EnumPopup("Item Type", vTarget.Type);

			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}

			GUI.enabled = true;
		}

	}

}
