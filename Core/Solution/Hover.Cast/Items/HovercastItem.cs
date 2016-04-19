using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Items {
	
	/*================================================================================================*/
	public class HovercastItem : MonoBehaviour, IHovercommonItem {

		public enum HovercastItemType {
			Parent,
			Selector,
			Sticky,
			Checkbox,
			Radio,
			Slider,
			Text
		}

		public HovercastItemType Type;
		public string Id = "";
		public string Label = "";
		public float RelativeSize = 1;
		public bool IsVisible = true;
		public bool IsEnabled = true;
		public bool NavigateBackUponSelect;

		public bool CheckboxValue;

		public bool RadioValue;

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
				case HovercastItemType.Checkbox:
					var checkItem = new CheckboxItem();
					checkItem.Value = CheckboxValue;
					vItem = checkItem;
					break;

				case HovercastItemType.Parent:
					vItem = new ParentItem(GetChildItems);
					break;

				case HovercastItemType.Radio:
					var radItem = new RadioItem(gameObject.transform.parent.name);
					radItem.Value = RadioValue;
					vItem = radItem;
					break;

				case HovercastItemType.Selector:
					vItem = new SelectorItem();
					break;

				case HovercastItemType.Slider:
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

				case HovercastItemType.Sticky:
					vItem = new StickyItem();
					break;

				case HovercastItemType.Text:
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
			vItem.Height = RelativeSize;
			vItem.IsVisible = IsVisible;
			vItem.IsEnabled = IsEnabled;

			ISelectorItem selItem = (vItem as ISelectorItem);

			if ( selItem != null ) {
				selItem.NavigateBackUponSelect = NavigateBackUponSelect;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			return HovercastItemHierarchy.GetChildItemsFromGameObject(gameObject);
		}

	}

}
