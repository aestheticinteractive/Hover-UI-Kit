using UnityEngine;

namespace Hovercast.Core.Navigation {
	
	/*================================================================================================*/
	public class HovercastNavItem : MonoBehaviour {

		public NavItem.ItemType Type;
		public string Label = "";
		public float RelativeSize = 1;
		public bool NavigateBackUponSelect;
		public bool ValueBool;
		public float ValueFloat;

		private NavItem vItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem GetItem() {
			if ( vItem == null ) {
				BuildItem();
			}

			FillItem();
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
			vItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			vItem.RelativeSize = RelativeSize;
			vItem.NavigateBackUponSelect = NavigateBackUponSelect;

			switch ( Type ) {
				case NavItem.ItemType.Checkbox:
				case NavItem.ItemType.Radio:
					((NavItem<bool>)vItem).Value = ValueBool;
					break;

				case NavItem.ItemType.Slider:
					NavItemSlider sliderItem = (NavItemSlider)vItem;
					sliderItem.Value = ValueFloat;
					break;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private NavItem[] GetChildItems() {
			return HovercastNavProvider.GetChildItems(gameObject);
		}

	}

}
