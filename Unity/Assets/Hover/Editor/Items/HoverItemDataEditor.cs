using Hover.Core.Items;
using Hover.Core.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Editor.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverItemData), true)]
	[CanEditMultipleObjects]
	public class HoverItemDataEditor : UnityEditor.Editor {

		private GUIStyle vVertStyle;

		private string vIsHiddenOpenKey;
		private string vIsEventOpenKey;
		
		private HoverItemData vData;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vVertStyle = EditorUtil.GetVerticalSectionStyle();
			
			int targetId = target.GetInstanceID();

			vIsHiddenOpenKey = "IsHiddenOpen"+targetId;
			vIsEventOpenKey = "IsEventOpen"+targetId;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vData = (HoverItemData)target;

			serializedObject.Update();
			DrawItems();
			serializedObject.ApplyModifiedProperties();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override bool RequiresConstantRepaint() {
			return EditorPrefs.GetBool(vIsHiddenOpenKey);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawItems() {
			DrawMainItems();
			DrawEventItemGroup();
			DrawHiddenItemGroup();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DrawMainItems() {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Id"), new GUIContent("ID"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Label"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_IsEnabled"));
			
			////

			HoverItemDataSelector selectorData = (vData as HoverItemDataSelector);
			HoverItemDataCheckbox checkboxData = (vData as HoverItemDataCheckbox);
			HoverItemDataRadio radioData = (vData as HoverItemDataRadio);
			HoverItemDataSlider sliderData = (vData as HoverItemDataSlider);
			HoverItemDataSticky stickyData = (vData as HoverItemDataSticky);
			
			if ( selectorData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Action"));
			}
			else if ( checkboxData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"),
					new GUIContent("Checkbox Value"));
			}
			else if ( radioData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_GroupId"),
					new GUIContent("Radio Group ID"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"),
					new GUIContent("Radio Value"));
			}
			else if ( sliderData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_LabelFormat"),
					new GUIContent("Slider Label Format"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_RangeMin"),
					new GUIContent("Slider Range Min"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_RangeMax"),
					new GUIContent("Slider Range Max"));

				float rangeValue = Mathf.Lerp(
					sliderData.RangeMin, sliderData.RangeMax, sliderData.Value);
				rangeValue = EditorGUILayout.Slider(
					"Slider Range Value", rangeValue, sliderData.RangeMin, sliderData.RangeMax);

				if ( sliderData.RangeValue != rangeValue ) {
					serializedObject.FindProperty("_Value").floatValue =
						Mathf.InverseLerp(sliderData.RangeMin, sliderData.RangeMax, rangeValue);
				}

				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Ticks"),
					new GUIContent("Slider Ticks"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Snaps"),
					new GUIContent("Slider Snaps"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_AllowJump"),
					new GUIContent("Slider Allow Jump"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_FillStartingPoint"),
					new GUIContent("Slider Fill Starting-Point"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_AllowIdleDeselection"),
					new GUIContent("Allow Idle Deselection"));
			}
			else if ( stickyData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_AllowIdleDeselection"),
					new GUIContent("Allow Idle Deselection"));
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawEventItemGroup() {
			HoverItemDataSelectable selectableData = (vData as HoverItemDataSelectable);

			if ( selectableData == null ) {
				return;
			}

			bool isEventOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsEventOpenKey), "Events");
			EditorPrefs.SetBool(vIsEventOpenKey, isEventOpen);
			
			if ( isEventOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				DrawEventItems();
				EditorGUILayout.EndVertical();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DrawEventItems() {
			HoverItemDataSelectableBool selBoolData = (vData as HoverItemDataSelectableBool);
			HoverItemDataSelectableFloat selFloatData = (vData as HoverItemDataSelectableFloat);
			
			EditorGUILayout.PropertyField(serializedObject.FindProperty("OnEnabledChangedEvent"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("OnSelectedEvent"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("OnDeselectedEvent"));

			if ( selBoolData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnValueChangedEvent"));
			}

			if ( selFloatData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnValueChangedEvent"));
			}
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawHiddenItemGroup() {
			bool isHiddenOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsHiddenOpenKey), "Info");
			EditorPrefs.SetBool(vIsHiddenOpenKey, isHiddenOpen);
			
			if ( isHiddenOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				GUI.enabled = false;
				DrawHiddenItems();
				GUI.enabled = true;
				EditorGUILayout.EndVertical();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DrawHiddenItems() {
			HoverItemDataSelectable selectableData = (vData as HoverItemDataSelectable);

			EditorGUILayout.IntField("Auto ID", vData.AutoId);
			EditorGUILayout.Toggle("Is Ancestry Enabled", vData.IsAncestryEnabled);
			EditorGUILayout.Toggle("Is Ancestry Visible", vData.IsAncestryVisible);

			if ( selectableData == null ) {
				return;
			}

			EditorGUILayout.Toggle("Is Sticky-Selected", selectableData.IsStickySelected);
			EditorGUILayout.Toggle("Ignore Selection", selectableData.IgnoreSelection);

			IItemDataRadio radioData = (vData as IItemDataRadio);
			IItemDataSlider sliderData = (vData as IItemDataSlider);

			if ( radioData != null ) {
				EditorGUILayout.TextField("Radio Default Group ID", radioData.DefaultGroupId);
			}

			if ( sliderData != null ) {
				EditorGUILayout.TextField("Slider Formatted Label", 
					sliderData.GetFormattedLabel(sliderData));
				EditorGUILayout.Slider("Slider Value", sliderData.Value, 0, 1);
				EditorGUILayout.Slider("Slider Snapped Value", sliderData.SnappedValue, 0, 1);
				EditorGUILayout.Slider("Slider Snapped Range Value", sliderData.SnappedRangeValue, 
					sliderData.RangeMin, sliderData.RangeMax);
				EditorGUILayout.TextField("Slider Hover Value", sliderData.HoverValue+"");
				EditorGUILayout.TextField("Slider Snapped Hover Value",sliderData.SnappedHoverValue+"");
			}
		}

	}

}
