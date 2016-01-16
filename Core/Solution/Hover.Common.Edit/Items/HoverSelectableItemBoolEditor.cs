using Hover.Common.Components.Items;
using Hover.Common.Components.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSelectableItemBool))]
	public abstract class HoverSelectableItemBoolEditor : HoverSelectableItemEditor {
	
		private SerializedProperty vOnValueChangedProp;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSelectableItemBool)target;
			string onValChangeName = GetPropertyName(() => t.OnValueChanged);
			
			vOnValueChangedProp = serializedObject.FindProperty(onValChangeName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSelectableItemBool)target;
			
			t.Value = EditorGUILayout.Toggle("Value", t.Value);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawEventItems() {
			base.DrawEventItems();
			
			EditorGUILayout.PropertyField(vOnValueChangedProp);
		}

	}

}
