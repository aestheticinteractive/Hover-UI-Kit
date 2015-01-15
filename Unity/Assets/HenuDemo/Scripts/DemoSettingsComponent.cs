using Henu.Navigation;
using Henu.Settings;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoSettingsComponent : HenuDefaultSettingsComponent {


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
				sett.HighlightColor = HighlightColor;
				return sett;
			}

			return base.GetArcSegmentSettings(pNavItem);
		}

	}

}
