using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;

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
			InteractionSettings sett = DemoSettingsComponent.InteractionSettings;
			sett.IsMenuOnLeftSide = !sett.IsMenuOnLeftSide;
		}

	}

}
