using Hover.Demo.CastCubes.Custom;
using UnityEditor;

namespace Assets.Hover.Demo.CastCubes.Custom.Editor {

	/*================================================================================================*/
	[CustomEditor(typeof(DemoHueItemVisualSettings))]
	public class DemoHueItemVisualSettingsEditor : UnityEditor.Editor {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			EditorGUILayout.HelpBox("This item automatically updates its color settings "+
				"based on the current slider value.", MessageType.Info);
		}

	}

}
