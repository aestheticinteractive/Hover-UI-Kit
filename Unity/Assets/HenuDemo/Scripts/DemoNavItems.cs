using System;
using System.Linq;
using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavItems {

		public NavItemParent Colors { get; private set; }
		public NavItemRadio ColorWhite { get; private set; }
		public NavItemRadio ColorRandom { get; private set; }
		public NavItemRadio ColorCustom { get; private set; }
		public NavItemSlider ColorHue { get; private set; }

		public NavItemParent Motions { get; private set; }
		public NavItem MotionOrbit { get; private set; }
		public NavItem MotionSpin { get; private set; }
		public NavItem MotionBob { get; private set; }
		public NavItem MotionGrow { get; private set; }

		public NavItemParent LightPos { get; private set; }
		public NavItemRadio LightPosHighest { get; private set; }
		public NavItemRadio LightPosHigh { get; private set; }
		public NavItemRadio LightPosLow { get; private set; }
		public NavItemRadio LightPosLowest { get; private set; }

		public NavItemParent LightInten { get; private set; }
		public NavItemRadio LightIntenHigh { get; private set; }
		public NavItemRadio LightIntenMed { get; private set; }
		public NavItemRadio LightIntenLow { get; private set; }

		public NavItemParent CameraPos { get; private set; }
		public NavItemRadio CameraPosCenter { get; private set; }
		public NavItemRadio CameraPosBack { get; private set; }
		public NavItemRadio CameraPosTop { get; private set; }
		public NavItemSelector CameraPosReorient { get; private set; }

		public NavItem[] TopLevelItems { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
			BuildMotions();
			BuildLightPos();
			BuildLightInten();
			BuildCameraPos();

			TopLevelItems = new[] { Colors, Motions, LightPos, LightInten, CameraPos };
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsItemWithin(NavItem pItem, NavItemParent pParent) {
			VerifyParent(pParent);
			return pParent.Children.Contains(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static NavItemRadio GetChosenRadioItem(NavItemParent pParent) {
			VerifyParent(pParent);

			foreach ( NavItem item in pParent.Children ) {
				NavItemRadio radItem = (item as NavItemRadio);

				if ( radItem != null && radItem.Value ) {
					return radItem;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyParent(NavItemParent pParent) {
			if ( pParent.Children != null && pParent.Children.Length != 0 ) {
				return;
			}

			throw new Exception("Parent item '"+pParent.Label+"' has no children.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			Colors = new NavItemParent("Cube Color");

			ColorWhite = new NavItemRadio("White");
			ColorRandom = new NavItemRadio("Random");
			ColorCustom = new NavItemRadio("Custom");
			ColorHue = new NavItemSlider("Hue", 4);
			ColorWhite.Value = true;
			ColorHue.IsEnabled = false;
			Colors.SetChildren(new NavItem[] { ColorWhite, ColorRandom, ColorCustom, ColorHue });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			Motions = new NavItemParent("Cube Motion");

			MotionOrbit = new NavItemCheckbox("Orbit");
			MotionSpin = new NavItemCheckbox("Spin");
			MotionBob = new NavItemCheckbox("Bob");
			MotionGrow = new NavItemCheckbox("Grow");
			Motions.SetChildren(new[] { MotionOrbit, MotionSpin, MotionBob, MotionGrow });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLightPos() {
			LightPos = new NavItemParent("Light Position");

			LightPosHighest = new NavItemRadio("Highest");
			LightPosHigh = new NavItemRadio("High");
			LightPosLow = new NavItemRadio("Low");
			LightPosLowest = new NavItemRadio("Lowest");
			LightPosHigh.Value = true;
			LightPos.SetChildren(new NavItem[] { LightPosHighest, LightPosHigh, LightPosLow, 
				LightPosLowest });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLightInten() {
			LightInten = new NavItemParent("Light Intensity");

			LightIntenHigh = new NavItemRadio("Brighest");
			LightIntenMed = new NavItemRadio("Medium");
			LightIntenLow = new NavItemRadio("Dimmest");
			LightIntenMed.Value = true;
			LightInten.SetChildren(new NavItem[] { LightIntenHigh, LightIntenMed, LightIntenLow });
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCameraPos() {
			CameraPos = new NavItemParent("Camera Position");

			CameraPosCenter = new NavItemRadio("Center");
			CameraPosBack = new NavItemRadio("Back");
			CameraPosTop = new NavItemRadio("Top");
			CameraPosReorient = new NavItemSelector("Re-orient");
			CameraPosCenter.Value = true;
			//CameraPosReorient.NavigateBackUponSelect = true;
			CameraPos.SetChildren(new NavItem[] { CameraPosCenter, CameraPosBack, CameraPosTop, 
				CameraPosReorient });
		}

	}

}
