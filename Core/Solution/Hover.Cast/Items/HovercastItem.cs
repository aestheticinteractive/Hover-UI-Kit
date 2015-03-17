using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Items {
	
	/*================================================================================================*/
	public class HovercastItem : MonoBehaviour {

		public SelectableItemType Type;
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
				case SelectableItemType.Checkbox:
					var checkItem = new CheckboxItem();
					checkItem.Value = CheckboxValue;
					vItem = checkItem;
					break;

				case SelectableItemType.Parent:
					vItem = new ParentItem(GetChildItems);
					break;

				case SelectableItemType.Radio:
					var radItem = new RadioItem();
					radItem.Value = RadioValue;
					vItem = radItem;
					break;

				case SelectableItemType.Selector:
					vItem = new SelectorItem();
					break;

				case SelectableItemType.Slider:
					var sliderItem = new SliderItem();
					sliderItem.Ticks = SliderTicks;
					sliderItem.Snaps = SliderSnaps;
					sliderItem.RangeMin = SliderRangeMin;
					sliderItem.RangeMax = SliderRangeMax;
					sliderItem.Value = Mathf.InverseLerp(SliderRangeMin, SliderRangeMax, SliderValue);
					sliderItem.AllowJump = SliderAllowJump;
					vItem = sliderItem;
					break;

				case SelectableItemType.Sticky:
					vItem = new StickyItem();
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

			ISelectableItem selItem = (vItem as ISelectableItem);

			if ( selItem != null && !(vItem is IParentItem) ) {
				selItem.NavigateBackUponSelect = NavigateBackUponSelect;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			return HovercastItemsProvider.GetChildItemsFromGameObject(gameObject);
		}

	}

}
