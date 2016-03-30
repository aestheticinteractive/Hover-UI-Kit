using Hover.Common.Components.Items.Types;
using Hover.Common.Items.Types;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items.Types {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverSliderItem))]
	public class HoverSliderItemEditor : HoverSelectableItemEditor {
	
		private SerializedProperty vOnValueChangedProp;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			
			var t = (HoverSliderItem)target;
			string onValChangeName = GetPropertyName(() => t.OnValueChangedEvent);
			
			vOnValueChangedProp = serializedObject.FindProperty(onValChangeName);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void DrawMainItems() {
			base.DrawMainItems();
			
			var t = (HoverSliderItem)target;
			float rangeValue = Mathf.Lerp(t.RangeMin, t.RangeMax, t.Value);
			
			t.RangeMin = EditorGUILayout.FloatField("Slider Range Min", t.RangeMin);
			t.RangeMax = EditorGUILayout.FloatField("Slider Range Max", t.RangeMax);
			rangeValue = EditorGUILayout.Slider(
				"Slider Range Value", rangeValue, t.RangeMin, t.RangeMax);
			t.Value = Mathf.InverseLerp(t.RangeMin, t.RangeMax, rangeValue);
			t.Ticks = EditorGUILayout.IntField("Slider Ticks", t.Ticks);
			t.Snaps = EditorGUILayout.IntField("Slider Snaps", t.Snaps);
			t.AllowJump = EditorGUILayout.Toggle("Slider Allow Jump", t.AllowJump);
			t.FillStartingPoint = (SliderItemFillType)EditorGUILayout.EnumPopup(
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
			
			EditorGUILayout.FloatField("Slider Value", t.Value);
			EditorGUILayout.FloatField("Slider Snapped Value", t.SnappedValue);
			EditorGUILayout.FloatField("Slider Snapped Range Value", t.SnappedRangeValue);
			EditorGUILayout.TextField("Slider Hover Value", t.HoverValue+"");
			EditorGUILayout.TextField("Slider Hover Snapped Value", t.HoverSnappedValue+"");
		}
		
	}

}
