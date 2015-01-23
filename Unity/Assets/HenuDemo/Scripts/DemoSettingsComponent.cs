using Henu.Navigation;
using Henu.Settings;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoSettingsComponent : HenuDefaultSettingsComponent {

		public static InteractionSettings InteractionSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			if ( pNavItem == DemoNavComponent.NavDelegate.Items.ColorHue ) {
				var sett = new DemoHueSliderSettings(pNavItem);
				sett.TextSize = TextSize;
				sett.TextColor = TextColor;
				sett.TextFont = TextFont;
				sett.ArrowIconColor = ArrowIconColor;
				sett.ToggleIconColor = ToggleIconColor;
				sett.BackgroundColor = BackgroundColor;
				sett.EdgeColor = EdgeColor;
				sett.HighlightColor = HighlightColor;
				sett.SliderTickColor = SliderTickColor;
				return sett;
			}

			return base.GetArcSegmentSettings(pNavItem);
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
