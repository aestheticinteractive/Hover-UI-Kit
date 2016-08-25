using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Hover.Editor.Utils {

	/*================================================================================================*/
	[CustomPropertyDrawer(typeof(DisableWhenControlledAttribute))]
	public class DisableWhenControlledPropertyDrawer : PropertyDrawer {

		private const int MinSingleRowVector3Width = 299;
		private const string Vector3TypeName = "Vector3";
		private const string IconTextPrefix = " _  ";
		private static readonly Texture2D ControlIconTex = 
			Resources.Load<Texture2D>("Textures/ControlledPropertyIconTexture");
		private static readonly Texture2D ControlIconHoverTex = 
			Resources.Load<Texture2D>("Textures/ControlledPropertyIconHoverTexture");

		private float vWidth;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnGUI(Rect pPosition, SerializedProperty pProp, GUIContent pLabel) {
			DisableWhenControlledAttribute attrib = (DisableWhenControlledAttribute)attribute;
			string mapName = attrib.ControllerMapName;
			SerializedObject self = pProp.serializedObject;
			ISettingsControllerMap map = EditorUtil.GetControllerMap(self, mapName);
			bool wasEnabled = GUI.enabled;
			Rect propRect = pPosition;
			bool hasRangeMin = (attrib.RangeMin != DisableWhenControlledAttribute.NullRangeMin);
			bool hasRangeMax = (attrib.RangeMax != DisableWhenControlledAttribute.NullRangeMax);
			bool isControlled = (map != null && map.IsControlled(pProp.name));
			string labelText = pLabel.text;
			
			if ( map != null && attrib.DisplaySpecials ) {
				List<string> specialValueNames = map.GetNewListOfControlledValueNames(true);
				Rect specialRect = propRect;
				specialRect.height = EditorGUIUtility.singleLineHeight;

				foreach ( string specialValueName in specialValueNames ) {
					DrawLinkIcon(self.targetObject, map.Get(specialValueName), specialRect);
					GUI.enabled = false;
					EditorGUI.LabelField(specialRect, IconTextPrefix+specialValueName.Substring(1));
					GUI.enabled = wasEnabled;
					specialRect.y += specialRect.height+EditorGUIUtility.standardVerticalSpacing;
				}

				propRect.y = specialRect.y;
				propRect.height = specialRect.height;
			}

			if ( isControlled ) {
				ISettingsController settingsController = map.Get(pProp.name);
				DrawLinkIcon(self.targetObject, settingsController, propRect);
				pLabel.text = IconTextPrefix+labelText;
			}
			else {
				pLabel.text = labelText;
			}

			GUI.enabled = !isControlled;
			vWidth = pPosition.width;

			if ( hasRangeMin && hasRangeMax ) {
				EditorGUI.Slider(propRect, pProp, attrib.RangeMin, attrib.RangeMax, pLabel);
			}
			else {
				EditorGUI.PropertyField(propRect, pProp, pLabel, true);

				if ( hasRangeMin ) {
					pProp.floatValue = Mathf.Max(pProp.floatValue, attrib.RangeMin);
				}
				else if ( hasRangeMax ) {
					pProp.floatValue = Mathf.Min(pProp.floatValue, attrib.RangeMax);
				}
			}

			GUI.enabled = wasEnabled;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float GetPropertyHeight(SerializedProperty pProp, GUIContent pLabel) {
			DisableWhenControlledAttribute attrib = (DisableWhenControlledAttribute)attribute;
			string mapName = attrib.ControllerMapName;
			ISettingsControllerMap map = EditorUtil.GetControllerMap(pProp.serializedObject, mapName);
			float propHeight = base.GetPropertyHeight(pProp, pLabel);

			if ( pProp.type == Vector3TypeName ) {
				return propHeight*(vWidth < MinSingleRowVector3Width ? 2 : 1);
			}

			if ( map == null || !attrib.DisplaySpecials ) {
				return propHeight;
			}

			float lineH = EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing;
			return lineH*map.GetControlledCount(true) + propHeight;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawLinkIcon(Object pSelf, ISettingsController pControl, Rect pPropertyRect) {
			bool isSelf = ((pControl as Object) == pSelf);

			Rect iconRect = pPropertyRect;
			iconRect.x -= 26;
			iconRect.y += 1;
			iconRect.width = 40;
			iconRect.height = 16;

			GUIContent labelContent = new GUIContent();
			labelContent.image = ControlIconTex;
			labelContent.tooltip = "Controlled by "+(isSelf ? "this component" : 
				pControl.GetType().Name+" in \""+pControl.name+"\"");

			GUIStyle labelStyle = new GUIStyle();
			labelStyle.imagePosition = ImagePosition.ImageOnly;
			labelStyle.clipping = TextClipping.Clip;
			labelStyle.padding = new RectOffset(15, 0, 0, 0);
			labelStyle.stretchWidth = true;
			labelStyle.stretchHeight = true;
			labelStyle.hover.background = ControlIconHoverTex;

			bool shouldPing = EditorGUI.ToggleLeft(iconRect, labelContent, false, labelStyle);

			if ( shouldPing ) {
				EditorGUIUtility.PingObject((Object)pControl);
			}
		}
		
	}

}
