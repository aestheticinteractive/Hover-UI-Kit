using System;
using System.Linq.Expressions;
using Hover.Common.Items;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CanEditMultipleObjects]
	[CustomEditor(typeof(HoverItemData))]
	public class HoverItemDataEditor : Editor {

		private HoverItemData vTarget;
		private GUIStyle vVertStyle;

		private string vIsHiddenOpenKey;
		private string vIsEventOpenKey;

		private SerializedProperty vOnSelectedProp;
		private SerializedProperty vOnDeselectedProp;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vTarget = (HoverItemData)target;

			vVertStyle = new GUIStyle();
			vVertStyle.padding = new RectOffset(16, 0, 0, 0);
			
			int targetId = vTarget.GetInstanceID();

			vIsHiddenOpenKey = "IsHiddenOpen"+targetId;
			vIsEventOpenKey = "IsEventOpen"+targetId;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(vTarget, vTarget.GetType().Name);
			
			FindProperties();
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
		private void FindProperties() {
			SelectableItem selectableData = (vTarget.Data as SelectableItem);

			if ( selectableData == null ) {
				vOnSelectedProp = null;
				vOnDeselectedProp = null;
				return;
			}

			/*if ( vOnSelectedProp == null ) {
				string dataPropName = GetPropertyName(() => vTarget.DataProp);
				string onSelName = GetPropertyName(() => selectableData.OnSelectedEvent);
				string onDeselName = GetPropertyName(() => selectableData.OnDeselectedEvent);
				SerializedProperty serializedProp = serializedObject.FindProperty(dataPropName);
				Debug.Log("prop: "+selectableData+" / "+dataPropName+" / "+serializedProp);

				if ( serializedProp == null ) {
					return; //TODO: temporary
				}

				vOnSelectedProp = serializedProp.FindPropertyRelative(onSelName);
				vOnDeselectedProp = serializedProp.FindPropertyRelative(onDeselName);
			}*/
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DrawItems() {
			DrawMainItems();
			DrawEventItemGroup();
			DrawHiddenItemGroup();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
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
			if ( vOnSelectedProp == null ) {
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
			//SelectableItem
			EditorGUILayout.PropertyField(vOnSelectedProp);
			EditorGUILayout.PropertyField(vOnDeselectedProp);
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
