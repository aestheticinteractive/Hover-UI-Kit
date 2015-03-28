using Hover.Demo.CastCubes.Custom;
using UnityEditor;

namespace Hovercast.Demo.Custom.Editor {

	/*================================================================================================*/
	[CustomEditor(typeof(DemoHueSegment))]
	public class DemoHueSegmentEditor : UnityEditor.Editor {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			EditorGUILayout.HelpBox("This item automatically updates its color settings "+
				"based on the current slider value.", MessageType.Info);
		}

	}

}
