using System;
using System.Linq;
using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavItems {

		public NavItemParent Color { get; private set; }
		public NavItemRadio ColorWhite { get; private set; }
		public NavItemRadio ColorRandom { get; private set; }
		public NavItemRadio ColorCustom { get; private set; }
		public NavItemSlider ColorHue { get; private set; }

		public NavItemParent Motion { get; private set; }
		public NavItemCheckbox MotionOrbit { get; private set; }
		public NavItemCheckbox MotionSpin { get; private set; }
		public NavItemCheckbox MotionBob { get; private set; }
		public NavItemCheckbox MotionGrow { get; private set; }
		public NavItemSlider MotionSpeed { get; private set; }

		public NavItemParent Light { get; private set; }
		public NavItemSlider LightPos { get; private set; }
		public NavItemSlider LightInten { get; private set; }
		public NavItemSticky LightSpot { get; private set; }

		public NavItemParent Camera { get; private set; }
		public NavItemRadio CameraCenter { get; private set; }
		public NavItemRadio CameraBack { get; private set; }
		public NavItemRadio CameraTop { get; private set; }
		public NavItemSelector CameraReorient { get; private set; }

		public NavItemParent Nested { get; private set; }
		public NavItemParent NestedA { get; private set; }
		public NavItemCheckbox NestedA1 { get; private set; }
		public NavItemCheckbox NestedA2 { get; private set; }
		public NavItemCheckbox NestedA3 { get; private set; }
		public NavItemParent NestedB { get; private set; }
		public NavItemCheckbox NestedB1 { get; private set; }
		public NavItemCheckbox NestedB2 { get; private set; }
		public NavItemCheckbox NestedB3 { get; private set; }
		public NavItemCheckbox NestedB4 { get; private set; }
		public NavItemParent NestedC { get; private set; }
		public NavItemCheckbox NestedC1 { get; private set; }
		public NavItemCheckbox NestedC2 { get; private set; }
		public NavItemCheckbox NestedC3 { get; private set; }
		public NavItemCheckbox NestedC4 { get; private set; }
		public NavItemCheckbox NestedC5 { get; private set; }

		public NavLevel TopLevel { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
			BuildMotions();
			BuildLight();
			BuildCamera();
			BuildNested();

			TopLevel = new NavLevel(Color, Motion, Light, Camera, Nested);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsItemWithin(NavItem pItem, NavItemParent pParent, NavItem.ItemType pType) {
			VerifyParent(pParent);
			return (pItem.Type == pType && pParent.ChildLevel.Items.Contains(pItem));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static NavItemRadio GetChosenRadioItem(NavItemParent pParent) {
			VerifyParent(pParent);

			foreach ( NavItem item in pParent.ChildLevel.Items ) {
				NavItemRadio radItem = (item as NavItemRadio);

				if ( radItem != null && radItem.Value ) {
					return radItem;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyParent(NavItemParent pParent) {
			if ( pParent.ChildLevel != null && pParent.ChildLevel.Items.Length != 0 ) {
				return;
			}

			throw new Exception("Parent item '"+pParent.Label+"' has no children.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			ColorWhite = new NavItemRadio("White");
			ColorRandom = new NavItemRadio("Random");
			ColorCustom = new NavItemRadio("Custom");
			ColorHue = new NavItemSlider("Hue", 3);

			Color = new NavItemParent("Color");
			Color.ChildLevel.Items = new NavItem[] { ColorWhite, ColorRandom, ColorCustom, ColorHue };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			MotionOrbit = new NavItemCheckbox("Orbit");
			MotionSpin = new NavItemCheckbox("Spin");
			MotionBob = new NavItemCheckbox("Bob");
			MotionGrow = new NavItemCheckbox("Grow");
			MotionSpeed = new NavItemSlider("Speed", 4);

			Motion = new NavItemParent("Motion");
			Motion.ChildLevel.Items = new NavItem[] { MotionOrbit, MotionSpin, MotionBob, MotionGrow, 
				MotionSpeed };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLight() {
			LightPos = new NavItemSlider("Position", 2);
			LightInten = new NavItemSlider("Intensity", 2);
			LightSpot = new NavItemSticky("Spotlight");

			Light = new NavItemParent("Lighting");
			Light.ChildLevel.Items = new NavItem[] { LightPos, LightInten, LightSpot };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCamera() {
			CameraCenter = new NavItemRadio("Center");
			CameraBack = new NavItemRadio("Back");
			CameraTop = new NavItemRadio("Top");
			CameraReorient = new NavItemSelector("Re-orient");

			Camera = new NavItemParent("Camera");
			Camera.ChildLevel.Items = new NavItem[] { CameraCenter, CameraBack, CameraTop, 
				CameraReorient };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildNested() {
			NestedA1 = new NavItemCheckbox("Checkbox A1");
			NestedA2 = new NavItemCheckbox("Checkbox A2");
			NestedA3 = new NavItemCheckbox("Checkbox A3");

			NestedA = new NavItemParent("Menu A");
			NestedA.ChildLevel.Items = new NavItem[] { NestedA1, NestedA2, NestedA3 };

			NestedB1 = new NavItemCheckbox("Checkbox B1");
			NestedB2 = new NavItemCheckbox("Checkbox B2");
			NestedB3 = new NavItemCheckbox("Checkbox B3");
			NestedB4 = new NavItemCheckbox("Checkbox B4");

			NestedB = new NavItemParent("Menu B");
			NestedB.ChildLevel.Items = new NavItem[] { NestedB1, NestedB2, NestedB3, NestedB4 };

			NestedC1 = new NavItemCheckbox("Checkbox C1");
			NestedC2 = new NavItemCheckbox("Checkbox C2");
			NestedC3 = new NavItemCheckbox("Checkbox C3");
			NestedC4 = new NavItemCheckbox("Checkbox C4");

			NestedC5 = new NavItemCheckbox("Checkbox C5");
			NestedC = new NavItemParent("Menu C");
			NestedC.ChildLevel.Items = new NavItem[] { NestedC1, NestedC2, NestedC3, NestedC4,
				NestedC5 };

			Nested = new NavItemParent("Nested Menu");
			Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB, NestedC };
		}

	}

}
