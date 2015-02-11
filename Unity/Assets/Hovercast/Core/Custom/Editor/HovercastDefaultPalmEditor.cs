using UnityEditor;
using UnityEngine;

namespace Hovercast.Core.Custom.Editor {

	/*================================================================================================*/
	[CustomEditor(typeof(HovercastDefaultPalm))]
	public class HovercastDefaultPalmEditor : UnityEditor.Editor {

		private HovercastDefaultPalm vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HovercastDefaultPalm)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			
			vTarget.InheritSettings = EditorGUILayout.Toggle("Inherit Settings", 
				vTarget.InheritSettings);

			if ( !vTarget.InheritSettings ) {
				vTarget.TextSize = EditorGUILayout.IntField("Text Size", vTarget.TextSize);
				vTarget.TextColor = EditorGUILayout.ColorField("Text Color", vTarget.TextColor);
				vTarget.TextFont = EditorGUILayout.TextField("Text Font", vTarget.TextFont);
				vTarget.BackgroundColor = EditorGUILayout.ColorField("Background Color",
					vTarget.BackgroundColor);
			}

			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}
		}

	}

}
