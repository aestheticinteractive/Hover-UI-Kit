using UnityEditor;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	[CustomEditor(typeof(HovercastNavParent))]
	public class HovercastNavParentEditor : Editor {

		private HovercastNavParent vTarget;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HovercastNavParent)target;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);

			vTarget.Label = EditorGUILayout.TextField("Label", vTarget.Label);
			vTarget.RelativeSize = EditorGUILayout.FloatField("Relative Size", vTarget.RelativeSize);
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}
		}

	}

}
