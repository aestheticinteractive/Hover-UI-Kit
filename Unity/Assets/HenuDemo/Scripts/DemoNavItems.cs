using System;
using System.Linq;
using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavItems {

		public NavItem Colors { get; private set; }
		public NavItem ColorWhite { get; private set; }
		public NavItem ColorRed { get; private set; }
		public NavItem ColorOrange { get; private set; }
		public NavItem ColorYellow { get; private set; }
		public NavItem ColorGreen { get; private set; }
		public NavItem ColorBlue { get; private set; }
		public NavItem ColorPurple { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsItemWithin(NavItem pItem, NavItem pParent) {
			if ( pParent.Children == null || pParent.Children.Length == 0 ) {
				throw new Exception("Item '"+pParent.Label+"' has no children.");
			}

			return pParent.Children.Contains(pItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			Colors = new NavItem(NavItem.ItemType.Parent, "Color");
			ColorWhite = new NavItem(NavItem.ItemType.Radio, "White");
			ColorWhite.Selected = true;
			ColorRed = new NavItem(NavItem.ItemType.Radio, "Red");
			ColorOrange = new NavItem(NavItem.ItemType.Radio, "Orange");
			ColorYellow = new NavItem(NavItem.ItemType.Radio, "Yellow");
			ColorGreen = new NavItem(NavItem.ItemType.Radio, "Green");
			ColorBlue = new NavItem(NavItem.ItemType.Radio, "Blue");
			ColorPurple = new NavItem(NavItem.ItemType.Radio, "Purple");

			Colors.SetChildren(new[] { ColorWhite, ColorRed, ColorOrange, ColorYellow,
				ColorGreen, ColorBlue, ColorPurple });
		}

	}

}
