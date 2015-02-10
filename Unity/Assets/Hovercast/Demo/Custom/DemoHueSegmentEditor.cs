using UnityEditor;

namespace Hovercast.Demo.Custom {

	/*================================================================================================*/
	[CustomEditor(typeof(DemoHueSegment))]
	public class DemoHueSegmentEditor : Editor {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			EditorGUILayout.HelpBox("This item automatically updates its color settings "+
				"based on the current slider value.", MessageType.Info);
		}

	}

}
