using System;
using System.Linq.Expressions;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverItemData))]
	public class HoverItemDataEditor : Editor {

		private string vOnSelectedEventName;
		private string vOnDeselectedEventName;
		private string vOnBoolValueChangedEventName;
		private string vOnFloatValueChangedEventName;
		private GUIStyle vVertStyle;

		private string vIsHiddenOpenKey;
		private string vIsEventOpenKey;
		
		private HoverItemData vTarget;
		private SerializedObject vSerializedData;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			CheckboxItem selBoolData = CreateInstance<CheckboxItem>();
			SliderItem selFloatData = CreateInstance<SliderItem>();

			vOnSelectedEventName = GetPropertyName(() => selBoolData.OnSelectedEvent);
			vOnDeselectedEventName = GetPropertyName(() => selBoolData.OnDeselectedEvent);
			vOnBoolValueChangedEventName = GetPropertyName(() => selBoolData.OnValueChangedEvent);
			vOnFloatValueChangedEventName = GetPropertyName(() => selFloatData.OnValueChangedEvent);

			vVertStyle = new GUIStyle();
			vVertStyle.padding = new RectOffset(16, 0, 0, 0);
			
			int targetId = target.GetInstanceID();

			vIsHiddenOpenKey = "IsHiddenOpen"+targetId;
			vIsEventOpenKey = "IsEventOpen"+targetId;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			vTarget = (HoverItemData)target;

			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			DrawItems();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(vTarget);
			}
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
			BaseItem baseData = vTarget.Data;
		
			vTarget.ItemType = (HoverItemData.HoverItemType)EditorGUILayout.EnumPopup(
				"Item Type", vTarget.ItemType);

			baseData.Id = EditorGUILayout.TextField("ID", baseData.Id);
			baseData.Label = EditorGUILayout.TextField("Label", baseData.Label);
			baseData.Width = EditorGUILayout.FloatField("Width", baseData.Width);
			baseData.Height = EditorGUILayout.FloatField("Height", baseData.Height);
			baseData.IsEnabled = EditorGUILayout.Toggle("Is Enabled", baseData.IsEnabled);
			baseData.IsVisible = EditorGUILayout.Toggle("Is Visible", baseData.IsVisible);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawEventItemGroup() {
			SelectableItem selectableData = (vTarget.Data as SelectableItem);

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
			vSerializedData = new SerializedObject(vTarget.Data);

			var onSelectedProp = vSerializedData.FindProperty(vOnSelectedEventName);
			var onDeselectedProp = vSerializedData.FindProperty(vOnDeselectedEventName);

			//SelectableItem
			EditorGUILayout.PropertyField(onSelectedProp);
			EditorGUILayout.PropertyField(onDeselectedProp);
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
			BaseItem baseData = vTarget.Data;
			SelectableItem selectableData = (baseData as SelectableItem);

			EditorGUILayout.IntField("Auto ID", baseData.AutoId);
			EditorGUILayout.Toggle("Is Ancestry Enabled", baseData.IsAncestryEnabled);
			EditorGUILayout.Toggle("Is Ancestry Visible", baseData.IsAncestryVisible);

			if ( selectableData == null ) {
				return;
			}

			EditorGUILayout.Toggle("Is Sticky-Selected", selectableData.IsStickySelected);
			EditorGUILayout.Toggle("Allow Selection", selectableData.AllowSelection);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetPropertyName<T>(Expression<Func<T>> pPropExpr) {
			return ((MemberExpression)pPropExpr.Body).Member.Name;
		}

	}

}
