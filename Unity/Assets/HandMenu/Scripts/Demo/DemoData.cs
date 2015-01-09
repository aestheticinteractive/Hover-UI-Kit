using System.Collections.Generic;
using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoData : INavDelegate {

		public NavItemData Colors { get; private set; }
		public NavItemData ColorWhite { get; private set; }
		public NavItemData ColorRed { get; private set; }
		public NavItemData ColorOrange { get; private set; }
		public NavItemData ColorYellow { get; private set; }
		public NavItemData ColorGreen { get; private set; }
		public NavItemData ColorBlue { get; private set; }
		public NavItemData ColorPurple { get; private set; }

		private readonly NavItemData[] vNavItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoData() {
			var items = new List<NavItemData>();
			
			////

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

			////

			vNavItems = new[] { Colors };
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData[] GetTopLevelItems() {
			return vNavItems;
		}

	}

}
