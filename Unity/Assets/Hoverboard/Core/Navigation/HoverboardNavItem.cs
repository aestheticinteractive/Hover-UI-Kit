using UnityEngine;

namespace Hoverboard.Core.Navigation {
	
	/*================================================================================================*/
	public class HoverboardNavItem : MonoBehaviour {

		public NavItem.ItemType Type;
		public string Id = "";
		public string Label = "";
		public int Width = 1;
		public bool IsVisible = true;
		public bool IsEnabled = true;

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
				case NavItem.ItemType.Selector:
					vItem = new NavItemSelector();
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

			vItem.Container = gameObject;
			vItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			vItem.Width = Width;
			vItem.IsVisible = IsVisible;
			vItem.IsEnabled = IsEnabled;
		}

	}

}
