using Hover.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Hover.Editor.Utils {

	/*================================================================================================*/
	[CustomPropertyDrawer(typeof(TriggerButtonAttribute))]
	public class TriggerButtonPropertyDrawer : PropertyDrawer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnGUI(Rect pPosition, SerializedProperty pProp, GUIContent pLabel) {
			TriggerButtonAttribute attrib = (TriggerButtonAttribute)attribute;
			string methodName = attrib.OnSelectedMethodName;
			SerializedObject self = pProp.serializedObject;

			pPosition.x += pPosition.width*0.03f;
			pPosition.y += pPosition.height*0.2f;
			pPosition.width *= 0.94f;
			pPosition.height *= 0.6f;

			if ( GUI.Button(pPosition, attrib.ButtonLabel) ) {
				pProp.boolValue = true;
				EditorUtil.CallMethod(self, methodName);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float GetPropertyHeight(SerializedProperty pProp, GUIContent pLabel) {
			return base.GetPropertyHeight(pProp, pLabel)*3;
		}

	}

}
