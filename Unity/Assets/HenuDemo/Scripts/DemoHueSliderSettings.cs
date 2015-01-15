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
			set {
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Color SliderTrackColor {
			get {
				return GetColor(0.2f);
			}
			set {
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Color SliderFillColor {
			get {
				return GetColor(0.2f);
			}
			set {
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Color GetColor(float pAlpha) {
			float value = ((NavItemSlider)vNavItem).CurrentValue;
			Color col = DemoEnvironment.HsvToColor(value*360, 1, 0.5f);
			col.a = pAlpha;
			return col;
		}

	}

}
