using Hover.Common.Components.Items;
using UnityEditor;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSelectableItem))]
	public abstract class HoverSelectableItemEditor : HoverBaseItemEditor {
	
		private string vIsEventOpenKey;
		private SerializedProperty vOnSelectedProp;
		private SerializedProperty vOnDeselectedProp;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSelectableItem)target;
			string onSelName = GetPropertyName(() => t._OnSelected);
			string onDeselName = GetPropertyName(() => t._OnDeselected);
			
			vIsEventOpenKey = "IsEventOpen"+target.GetInstanceID();
			vOnSelectedProp = serializedObject.FindProperty(onSelName);
			vOnDeselectedProp = serializedObject.FindProperty(onDeselName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawEventItemGroup() {
			base.DrawEventItemGroup();
		
			bool isEventOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsEventOpenKey), "Events");
			EditorPrefs.SetBool(vIsEventOpenKey, isEventOpen);
			
			if ( isEventOpen ) {
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
			
			EditorGUILayout.Toggle("Is Sticky-Selected", t.IsStickySelected);
			EditorGUILayout.Toggle("Allow Selection", t.AllowSelection);
		}

	}

}
