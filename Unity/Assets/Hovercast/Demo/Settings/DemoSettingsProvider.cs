using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;

namespace Hovercast.Demo.Settings {

	/*================================================================================================*/
	public class DemoSettingsProvider : HovercastDefaultSettingsProvider {

		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			ArcSegmentSettings sett = base.GetArcSegmentSettings(pNavItem);

			if ( pNavItem != null && pNavItem.Id == "HUE" ) {
				var hueSett = new DemoHueSliderSettings(pNavItem);
				hueSett.Fill(hueSett);
				return hueSett;
			}

			return sett;
		}

	}

}
