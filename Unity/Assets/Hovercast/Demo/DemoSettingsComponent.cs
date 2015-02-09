using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoSettingsComponent : HovercastDefaultSettingsComponent {

		public static ArcSegmentSettings ArcSegmentSettings;
		public static InteractionSettings InteractionSettings;
		public static float BgAlpha;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			if ( ArcSegmentSettings == null ) {
				ArcSegmentSettings = base.GetArcSegmentSettings(null);
			}
			
			if ( pNavItem != null && pNavItem.Id == "HUE" ) {
				var sett = new DemoHueSliderSettings(pNavItem);
				sett.Fill(ArcSegmentSettings);
				return sett;
			}

			return ArcSegmentSettings;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override InteractionSettings  GetInteractionSettings() {
			if ( InteractionSettings == null ) {
				InteractionSettings = base.GetInteractionSettings();
			}

			return InteractionSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void UpdateSettingsWithBgAlpha() {
			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;

			Color c = sett.BackgroundColor;
			c.a = BgAlpha;
			sett.BackgroundColor = c;

			c = sett.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			sett.SliderFillColor = c;
		}

	}

}
