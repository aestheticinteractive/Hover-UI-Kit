using Hover.Common.Components.Items;
using UnityEditor;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSelectableItemBool))]
	public abstract class HoverSelectableItemBoolEditor : HoverSelectableItemEditor {
	
		private SerializedProperty vOnValueChangedProp;
		protected string vValueLabel;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSelectableItemBool)target;
			string onValChangeName = GetPropertyName(() => t.OnValueChangedEvent);
			
			vOnValueChangedProp = serializedObject.FindProperty(onValChangeName);
			vValueLabel = "Bool Value";
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSelectableItemBool)target;
			
			t.Value = EditorGUILayout.Toggle(vValueLabel, t.Value);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawEventItems() {
			base.DrawEventItems();
			
			EditorGUILayout.PropertyField(vOnValueChangedProp);
		}

	}

}
