using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoSettingsComponent : HovercastDefaultSettingsComponent {

		public static ArcSegmentSettings ArcSegmentSettings;
		public static InteractionSettings InteractionSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			if ( ArcSegmentSettings == null ) {
				ArcSegmentSettings = base.GetArcSegmentSettings(null);
			}
			
			if ( pNavItem == DemoNavComponent.NavDelegate.Items.ColorHue ) {
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

	}

}
