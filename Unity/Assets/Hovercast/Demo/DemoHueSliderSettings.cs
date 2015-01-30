using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoHueSliderSettings : ArcSegmentSettings {

		private readonly NavItem vNavItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoHueSliderSettings(NavItem pNavItem) {
			vNavItem = pNavItem;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Fill(ArcSegmentSettings pSettings) {
			TextSize = pSettings.TextSize;
			TextColor = pSettings.TextColor;
			TextFont = pSettings.TextFont;
			ArrowIconColor = pSettings.ArrowIconColor;
			ToggleIconColor = pSettings.ToggleIconColor;
			BackgroundColor = pSettings.BackgroundColor;
			EdgeColor = pSettings.EdgeColor;
			HighlightColor = pSettings.HighlightColor;
			SliderTickColor = pSettings.SliderTickColor;
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
