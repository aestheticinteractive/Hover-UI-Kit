using System;
using System.Linq.Expressions;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Editor.Items {

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

			SelectorItem selectorData = (vData as SelectorItem);
			CheckboxItem checkboxData = (vData as CheckboxItem);
			RadioItem radioData = (vData as RadioItem);
			SliderItem sliderData = (vData as SliderItem);

			if ( selectorData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_NavigateBackUponSelect"));
			}

			if ( checkboxData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"),
					new GUIContent("Checkbox Value"));
			}

			if ( radioData != null ) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_GroupId"),
					new GUIContent("Radio Group ID"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"),
					new GUIContent("Radio Value"));
			}

			if ( sliderData != null ) {
				float rangeValue = Mathf.Lerp(
					sliderData.RangeMin, sliderData.RangeMax, sliderData.Value);

				sliderData.LabelFormat = EditorGUILayout.TextField(
					"Slider Label Format", sliderData.LabelFormat);
				sliderData.RangeMin = EditorGUILayout.FloatField(
					"Slider Range Min", sliderData.RangeMin);
				sliderData.RangeMax = EditorGUILayout.FloatField(
					"Slider Range Max", sliderData.RangeMax);
				rangeValue = EditorGUILayout.Slider(
					"Slider Range Value", rangeValue, sliderData.RangeMin, sliderData.RangeMax);
				sliderData.Value = Mathf.InverseLerp(
					sliderData.RangeMin, sliderData.RangeMax, rangeValue);
				sliderData.Ticks = EditorGUILayout.IntField(
					"Slider Ticks", sliderData.Ticks);
				sliderData.Snaps = EditorGUILayout.IntField(
					"Slider Snaps", sliderData.Snaps);
				sliderData.AllowJump = EditorGUILayout.Toggle(
					"Slider Allow Jump", sliderData.AllowJump);
				sliderData.FillStartingPoint = (SliderItem.FillType)EditorGUILayout.EnumPopup(
					"Slider Fill Starting-Point", sliderData.FillStartingPoint);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawEventItemGroup() {
			SelectableItem selectableData = (vData as SelectableItem);

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
			SelectableItemBool selBoolData = (vData as SelectableItemBool);
			SelectableItemFloat selFloatData = (vData as SelectableItemFloat);
			
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
			SelectableItem selectableData = (vData as SelectableItem);

			EditorGUILayout.IntField("Auto ID", vData.AutoId);
			EditorGUILayout.Toggle("Is Ancestry Enabled", vData.IsAncestryEnabled);
			EditorGUILayout.Toggle("Is Ancestry Visible", vData.IsAncestryVisible);

			if ( selectableData == null ) {
				return;
			}

			EditorGUILayout.Toggle("Is Sticky-Selected", selectableData.IsStickySelected);
			EditorGUILayout.Toggle("Allow Selection", selectableData.AllowSelection);

			IRadioItem radioData = (vData as IRadioItem);
			ISliderItem sliderData = (vData as ISliderItem);

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
