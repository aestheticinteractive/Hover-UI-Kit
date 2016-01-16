using Hover.Common.Components.Items;
using Hover.Common.Components.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSelectableItem))]
	public abstract class HoverSelectableItemEditor : HoverBaseItemEditor {
	
		private bool vIsEventOpen;
		private SerializedProperty vOnSelectedProp;
		private SerializedProperty vOnDeselectedProp;
		
		protected bool vHideNavBack;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSelectableItem)target;
			string onSelName = GetPropertyName(() => t.OnSelected);
			string onDeselName = GetPropertyName(() => t.OnDeselected);
			
			vOnSelectedProp = serializedObject.FindProperty(onSelName);
			vOnDeselectedProp = serializedObject.FindProperty(onDeselName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSelectableItem)target;
			
			if ( !vHideNavBack ) {
				t.NavigateBackUponSelect = EditorGUILayout.Toggle(
					"Navigate Back Upon Select", t.NavigateBackUponSelect);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawEventItemGroup() {
			base.DrawEventItemGroup();
		
			vIsEventOpen = EditorGUILayout.Foldout(vIsEventOpen, "Events");
			
			if ( vIsEventOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				DrawEventItems();
				EditorGUILayout.EndVertical();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawEventItems() {
			EditorGUILayout.PropertyField(vOnSelectedProp);
			EditorGUILayout.PropertyField(vOnDeselectedProp);
		}
				
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawHiddenItems() {
			base.DrawHiddenItems();
			
			var t = (HoverSelectableItem)target;
			
			GUI.enabled = false;
			EditorGUILayout.Toggle("Is Sticky-Selected", t.Item.IsStickySelected);
			EditorGUILayout.Toggle("Allow Selection", t.Item.AllowSelection);
			GUI.enabled = true;
		}

	}

}
