using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomSwitchListener : DemoBaseListener<NavItemSelector> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.OnSelected += HandleSelected;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(NavItem pNavItem) {
			InteractionSettings sett = DemoSettingsComponent.InteractionSettings;
			sett.IsMenuOnLeftSide = !sett.IsMenuOnLeftSide;
		}

	}

}
