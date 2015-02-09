using UnityEngine;

namespace Hovercast.Core.Navigation {
	
	/*================================================================================================*/
	public class HovercastNavItem : MonoBehaviour {

		public NavItem.ItemType Type;
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

		private NavItem vItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem GetItem() {
			if ( vItem == null ) {
				BuildItem();
				FillItem();
			}

			return vItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildItem() {
			switch ( Type ) {
				case NavItem.ItemType.Checkbox:
					vItem = new NavItemCheckbox();
					break;

				case NavItem.ItemType.Parent:
					vItem = new NavItemParent(GetChildItems);
					break;

				case NavItem.ItemType.Radio:
					vItem = new NavItemRadio();
					break;

				case NavItem.ItemType.Selector:
					vItem = new NavItemSelector();
					break;

				case NavItem.ItemType.Slider:
					vItem = new NavItemSlider();
					break;

				case NavItem.ItemType.Sticky:
					vItem = new NavItemSticky();
					break;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FillItem() {
			if ( !string.IsNullOrEmpty(Id) ) {
				vItem.Id = Id;
			}

			vItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			vItem.RelativeSize = RelativeSize;
			vItem.IsVisible = IsVisible;
			vItem.IsEnabled = IsEnabled;

			if ( Type != NavItem.ItemType.Parent ) {
				vItem.NavigateBackUponSelect = NavigateBackUponSelect;
			}

			switch ( Type ) {
				case NavItem.ItemType.Checkbox:
					((NavItemCheckbox)vItem).Value = CheckboxValue;
					break;

				case NavItem.ItemType.Radio:
					((NavItemRadio)vItem).Value = RadioValue;
					break;

				case NavItem.ItemType.Slider:
					NavItemSlider sliderItem = (NavItemSlider)vItem;
					sliderItem.Ticks = SliderTicks;
					sliderItem.Snaps = SliderSnaps;
					sliderItem.RangeMin = SliderRangeMin;
					sliderItem.RangeMax = SliderRangeMax;
					sliderItem.Value = Mathf.InverseLerp(SliderRangeMin, SliderRangeMax, SliderValue);
					break;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private NavItem[] GetChildItems() {
			return HovercastNavProvider.GetChildItems(gameObject);
		}

	}

}
