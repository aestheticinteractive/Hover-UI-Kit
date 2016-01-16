using Hover.Common.Components.Items.Types;
using UnityEditor;
using Hover.Common.Items.Types;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSliderItem))]
	public class HoverSliderItemEditor : HoverSelectableItemEditor {
	
		//TOOD: RangeValue slider and the "Hover" hidden items don't update in realtime
	
		private SerializedProperty vOnValueChangedProp;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSliderItem)target;
			string onValChangeName = GetPropertyName(() => t.OnValueChanged);
			
			vOnValueChangedProp = serializedObject.FindProperty(onValChangeName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSliderItem)target;
			
			t.RangeMin = EditorGUILayout.FloatField("Slider Range Min", t.RangeMin);
			t.RangeMax = EditorGUILayout.FloatField("Slider Range Max", t.RangeMax);
			t.RangeValue = EditorGUILayout.Slider(
				"Slider Range Value", t.RangeValue, t.RangeMin, t.RangeMax);
			t.Ticks = EditorGUILayout.IntField("Slider Ticks", t.Ticks);
			t.Snaps = EditorGUILayout.IntField("Slider Snaps", t.Snaps);
			t.AllowJump = EditorGUILayout.Toggle("Slider Allow Jump", t.AllowJump);
			t.FillStartingPoint = (SliderItem.FillType)EditorGUILayout.EnumPopup(
				"Slider Fill Starting-Point", t.FillStartingPoint);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawEventItems() {
			base.DrawEventItems();
			
			EditorGUILayout.PropertyField(vOnValueChangedProp);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawHiddenItems() {
			base.DrawHiddenItems();
			
			var t = (HoverSliderItem)target;
			
			EditorGUILayout.FloatField("Slider Value", t.Item.Value);
			EditorGUILayout.FloatField("Slider Snapped Value", t.Item.SnappedValue);
			EditorGUILayout.FloatField("Slider Range Snapped Value", t.Item.RangeSnappedValue);
			EditorGUILayout.TextField("Slider Hover Value", t.Item.HoverValue+"");
			EditorGUILayout.TextField("Slider Hover Snapped Value", t.Item.HoverSnappedValue+"");
		}
		
	}

}
