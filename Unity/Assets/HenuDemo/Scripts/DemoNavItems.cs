using System;
using System.Linq;
using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavItems {

		public NavItem Colors { get; private set; }
		public NavItem ColorWhite { get; private set; }
		public NavItem ColorRed { get; private set; }
		public NavItem ColorYellow { get; private set; }
		public NavItem ColorGreen { get; private set; }
		public NavItem ColorBlue { get; private set; }
		public NavItem ColorRandLt { get; private set; }
		public NavItem ColorRandDk { get; private set; }

		public NavItem Motions { get; private set; }
		public NavItem MotionOrbit { get; private set; }
		public NavItem MotionSpin { get; private set; }
		public NavItem MotionBob { get; private set; }
		public NavItem MotionGrow { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
			BuildMotions();
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
			Colors = new NavItem(NavItem.ItemType.Parent, "Cube Color");
			ColorWhite = new NavItem(NavItem.ItemType.Radio, "White");
			ColorWhite.Selected = true;
			ColorRed = new NavItem(NavItem.ItemType.Radio, "Red");
			ColorYellow = new NavItem(NavItem.ItemType.Radio, "Yellow");
			ColorGreen = new NavItem(NavItem.ItemType.Radio, "Green");
			ColorBlue = new NavItem(NavItem.ItemType.Radio, "Blue");
			ColorRandLt = new NavItem(NavItem.ItemType.Radio, "Random Light");
			ColorRandDk = new NavItem(NavItem.ItemType.Radio, "Random Dark");

			Colors.SetChildren(new[] { ColorWhite, ColorRed, ColorYellow, ColorGreen, ColorBlue,
				ColorRandLt, ColorRandDk });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			Motions = new NavItem(NavItem.ItemType.Parent, "Cube Motion");
			MotionOrbit = new NavItem(NavItem.ItemType.Checkbox, "Orbit");
			MotionSpin = new NavItem(NavItem.ItemType.Checkbox, "Spin");
			MotionBob = new NavItem(NavItem.ItemType.Checkbox, "Bob");
			MotionGrow = new NavItem(NavItem.ItemType.Checkbox, "Grow");

			Motions.SetChildren(new[] { MotionOrbit, MotionSpin, MotionBob, MotionGrow });
		}

	}

}
