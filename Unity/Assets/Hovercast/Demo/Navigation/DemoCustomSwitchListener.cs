using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using Hovercast.Demo.Settings;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomSwitchListener : DemoBaseListener<NavItemSelector> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnSelected += HandleSelected;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//do nothing...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(NavItem pNavItem) {
			InteractionSettings sett = DemoSettingsProvider.InteractionSettings;
			sett.IsMenuOnLeftSide = !sett.IsMenuOnLeftSide;
		}

	}

}
