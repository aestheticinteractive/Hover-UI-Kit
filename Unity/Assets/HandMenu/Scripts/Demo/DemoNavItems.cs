using System;
using System.Linq;
using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoNavItems {

		public NavItemData Colors { get; private set; }
		public NavItemData ColorWhite { get; private set; }
		public NavItemData ColorRed { get; private set; }
		public NavItemData ColorOrange { get; private set; }
		public NavItemData ColorYellow { get; private set; }
		public NavItemData ColorGreen { get; private set; }
		public NavItemData ColorBlue { get; private set; }
		public NavItemData ColorPurple { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsItemWithin(NavItemData pItem, NavItemData pParent) {
			if ( pParent.Children == null || pParent.Children.Length == 0 ) {
				throw new Exception("Item '"+pParent.Label+"' has no children.");
			}

			return pParent.Children.Contains(pItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			Colors = new NavItemData(NavItemData.ItemType.Parent, "Color");
			ColorWhite = new NavItemData(NavItemData.ItemType.Radio, "White");
			ColorWhite.Selected = true;
			ColorRed = new NavItemData(NavItemData.ItemType.Radio, "Red");
			ColorOrange = new NavItemData(NavItemData.ItemType.Radio, "Orange");
			ColorYellow = new NavItemData(NavItemData.ItemType.Radio, "Yellow");
			ColorGreen = new NavItemData(NavItemData.ItemType.Radio, "Green");
			ColorBlue = new NavItemData(NavItemData.ItemType.Radio, "Blue");
			ColorPurple = new NavItemData(NavItemData.ItemType.Radio, "Purple");

			Colors.SetChildren(new[] { ColorWhite, ColorRed, ColorOrange, ColorYellow,
				ColorGreen, ColorBlue, ColorPurple });
		}

	}

}
