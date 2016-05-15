using Hover.Common.Renderers;
using Hover.Common.Util;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Utils {

	/*================================================================================================*/
	[CustomPropertyDrawer(typeof(DisableWhenControlledAttribute))]
	public class DisableWhenControlledPropertyDrawer : PropertyDrawer {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnGUI(Rect pPosition, SerializedProperty pProp, GUIContent pLabel) {
			DisableWhenControlledAttribute attrib = (DisableWhenControlledAttribute)attribute;
			string mapName = attrib.ControllerMapName;
			ISettingsControllerMap map = EditorUtil.GetControllerMap(pProp.serializedObject, mapName);
			RangeAttribute range = EditorUtil.GetFieldRangeAttribute(pProp);
			bool wasEnabled = GUI.enabled;
			Rect propRect = pPosition;
			float messageH = 0;

			if ( map != null && attrib.DisplayMessage && map.AreAnyControlled() ) {
				string message = RendererHelper.GetControlledSettingsText(map);
				Rect helpRect = pPosition;

				messageH = GetMessageHeight(map);
				helpRect.height = messageH;

				EditorGUI.HelpBox(helpRect, message, MessageType.Warning);
			}

			propRect.y += messageH+EditorGUIUtility.standardVerticalSpacing;
			propRect.height -= messageH;

			GUI.enabled = (map == null || !map.IsControlled(pProp.name));

			if ( range != null ) {
				EditorGUI.Slider(propRect, pProp, range.min, range.max);
			}
			else {
				EditorGUI.PropertyField(propRect, pProp, pLabel, true);
			}

			GUI.enabled = wasEnabled;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override float GetPropertyHeight(SerializedProperty pProp, GUIContent pLabel) {
			DisableWhenControlledAttribute attrib = (DisableWhenControlledAttribute)attribute;
			string mapName = attrib.ControllerMapName;
			ISettingsControllerMap map = EditorUtil.GetControllerMap(pProp.serializedObject, mapName);

			if ( map == null || !attrib.DisplayMessage || !map.AreAnyControlled() ) {
				return base.GetPropertyHeight(pProp, pLabel);
			}

			return GetMessageHeight(map) + EditorGUIUtility.standardVerticalSpacing + 
				EditorGUI.GetPropertyHeight(pProp, pLabel);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static float GetMessageHeight(ISettingsControllerMap pMap) {
			float lineCount = pMap.GetControlledCount()+1.5f;
			return EditorGUIUtility.singleLineHeight*0.7f*lineCount;
		}
		
	}

}
