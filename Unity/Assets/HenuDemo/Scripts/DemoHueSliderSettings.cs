using Henu.Navigation;
using HenuDemo;
using UnityEngine;

namespace Henu.Settings {

	/*================================================================================================*/
	public class DemoHueSliderSettings : ArcSegmentSettings {

		private readonly NavItem vNavItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoHueSliderSettings(NavItem pNavItem) {
			vNavItem = pNavItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Color SelectionColor {
			get {
				return GetColor(1);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Color SliderTrackColor {
			get {
				return GetColor(0.25f);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Color SliderFillColor {
			get {
				return GetColor(0.25f);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Color GetColor(float pAlpha) {
			float value = ((NavItemSlider)vNavItem).Value;
			Color col = DemoEnvironment.HsvToColor(value*360, 1, 0.666f);
			col.a = pAlpha;
			return col;
		}

	}

}
