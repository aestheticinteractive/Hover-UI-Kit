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

		public NavItem LightPos { get; private set; }
		public NavItem LightPosHighest { get; private set; }
		public NavItem LightPosHigh { get; private set; }
		public NavItem LightPosLow { get; private set; }
		public NavItem LightPosLowest { get; private set; }

		public NavItem LightInten { get; private set; }
		public NavItem LightIntenHigh { get; private set; }
		public NavItem LightIntenMed { get; private set; }
		public NavItem LightIntenLow { get; private set; }

		public NavItem CameraPos { get; private set; }
		public NavItem CameraPosCenter { get; private set; }
		public NavItem CameraPosBack { get; private set; }
		public NavItem CameraPosTop { get; private set; }
		public NavItem CameraPosReorient { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
			BuildMotions();
			BuildLightPos();
			BuildLightInten();
			BuildCameraPos();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsItemWithin(NavItem pItem, NavItem pParent) {
			VerifyParent(pParent);
			return pParent.Children.Contains(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static NavItem GetFirstSelectedChildItem(NavItem pParent) {
			VerifyParent(pParent);
			return pParent.Children.FirstOrDefault(x => x.Selected);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyParent(NavItem pParent) {
			if ( pParent.Children != null && pParent.Children.Length != 0 ) {
				return;
			}

			throw new Exception("Item '"+pParent.Label+"' has no children.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			Colors = new NavItem(NavItem.ItemType.Parent, "Cube Color");

			ColorWhite = new NavItem(NavItem.ItemType.Radio, "White");
			ColorRed = new NavItem(NavItem.ItemType.Radio, "Red");
			ColorYellow = new NavItem(NavItem.ItemType.Radio, "Yellow");
			ColorGreen = new NavItem(NavItem.ItemType.Radio, "Green");
			ColorBlue = new NavItem(NavItem.ItemType.Radio, "Blue");
			ColorRandLt = new NavItem(NavItem.ItemType.Radio, "Random Light");
			ColorRandDk = new NavItem(NavItem.ItemType.Radio, "Random Dark");
			ColorWhite.Selected = true;
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

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLightPos() {
			LightPos = new NavItem(NavItem.ItemType.Parent, "Light Position");

			LightPosHighest = new NavItem(NavItem.ItemType.Radio, "Highest");
			LightPosHigh = new NavItem(NavItem.ItemType.Radio, "High");
			LightPosLow = new NavItem(NavItem.ItemType.Radio, "Low");
			LightPosLowest = new NavItem(NavItem.ItemType.Radio, "Lowest");
			LightPosHigh.Selected = true;
			LightPos.SetChildren(new[] { LightPosHighest, LightPosHigh, LightPosLow, LightPosLowest });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLightInten() {
			LightInten = new NavItem(NavItem.ItemType.Parent, "Light Intensity");

			LightIntenHigh = new NavItem(NavItem.ItemType.Radio, "Brighest");
			LightIntenMed = new NavItem(NavItem.ItemType.Radio, "Medium");
			LightIntenLow = new NavItem(NavItem.ItemType.Radio, "Dimmest");
			LightIntenMed.Selected = true;
			LightInten.SetChildren(new[] { LightIntenHigh, LightIntenMed, LightIntenLow });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCameraPos() {
			CameraPos = new NavItem(NavItem.ItemType.Parent, "Camera Position");

			CameraPosCenter = new NavItem(NavItem.ItemType.Radio, "Center");
			CameraPosBack = new NavItem(NavItem.ItemType.Radio, "Back");
			CameraPosTop = new NavItem(NavItem.ItemType.Radio, "Top");
			CameraPosReorient = new NavItem(NavItem.ItemType.Selection, "Re-orient");
			CameraPosCenter.Selected = true;
			//CameraPosReorient.NavigateBackUponSelect = true;
			CameraPos.SetChildren(new[] { CameraPosCenter, CameraPosBack, CameraPosTop, 
				CameraPosReorient });
		}

	}

}
