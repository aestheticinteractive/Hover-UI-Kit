using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo.Settings {

	/*================================================================================================*/
	public class DemoSettingsProvider : HovercastDefaultSettingsProvider {

		public static ArcSegmentSettings ArcSegmentSettings;
		public static InteractionSettings InteractionSettings;
		public static float BgAlpha;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoSettingsProvider() {
			ArcSegmentSettings = vArcSegment;
			InteractionSettings = vInteraction;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			if ( pNavItem != null && pNavItem.Id == "HUE" ) {
				var sett = new DemoHueSliderSettings(pNavItem);
				sett.Fill(ArcSegmentSettings);
				return sett;
			}

			return ArcSegmentSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void UpdateSettingsWithBgAlpha() {
			ArcSegmentSettings sett = ArcSegmentSettings;

			Color c = sett.BackgroundColor;
			c.a = BgAlpha;
			sett.BackgroundColor = c;

			c = sett.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			sett.SliderFillColor = c;
		}

	}

}
