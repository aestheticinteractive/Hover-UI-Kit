using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Board.Items {
	
	/*================================================================================================*/
	public class HoverboardItem : MonoBehaviour, IHovercommonItem {

		public enum HoverboardItemType {
			Selector = 1, //legacy value from SelectableItemType.Selector
			Sticky,
			Checkbox,
			Radio,
			Slider,
			Text
		}

		public HoverboardItemType Type = HoverboardItemType.Selector;
		public string Id = "";
		public string Label = "";
		public float Width = 1;
		public float Height = 1;
		public bool IsVisible = true;
		public bool IsEnabled = true;

		public bool CheckboxValue;

		public bool RadioValue;
		public string RadioGroupId;

		public int SliderTicks;
		public int SliderSnaps;
		public float SliderRangeMin;
		public float SliderRangeMax = 1;
		public float SliderValue;
		public bool SliderAllowJump = true;
		public SliderItem.FillType SliderFillStartingPoint;

		private BaseItem vItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem GetItem() {
			if ( vItem == null ) {
				BuildItem();
			}

			return vItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildItem() {
			switch ( Type ) {
				case HoverboardItemType.Selector:
					vItem = new SelectorItem();
					break;

				case HoverboardItemType.Sticky:
					vItem = new StickyItem();
					break;

				case HoverboardItemType.Checkbox:
					var checkItem = new CheckboxItem();
					checkItem.Value = CheckboxValue;
					vItem = checkItem;
					break;

				case HoverboardItemType.Radio:
					var radItem = new RadioItem(gameObject.transform.parent.name);
					radItem.Value = RadioValue;
					radItem.GroupId = RadioGroupId;
					vItem = radItem;
					break;

				case HoverboardItemType.Slider:
					var sliderItem = new SliderItem();
					sliderItem.Ticks = SliderTicks;
					sliderItem.Snaps = SliderSnaps;
					sliderItem.RangeMin = SliderRangeMin;
					sliderItem.RangeMax = SliderRangeMax;
					sliderItem.Value = Mathf.InverseLerp(SliderRangeMin, SliderRangeMax, SliderValue);
					sliderItem.AllowJump = SliderAllowJump;
					sliderItem.FillStartingPoint = SliderFillStartingPoint;
					vItem = sliderItem;
					break;

				case HoverboardItemType.Text:
					vItem = new TextItem();
					break;

				default:
					throw new Exception("Unhandled item type: "+Type);
			}

			if ( !string.IsNullOrEmpty(Id) ) {
				vItem.Id = Id;
			}

			vItem.DisplayContainer = gameObject;
			vItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			vItem.Width = Width;
			vItem.Height = Height;
			vItem.IsVisible = IsVisible;
			vItem.IsEnabled = IsEnabled;
		}

	}

}
