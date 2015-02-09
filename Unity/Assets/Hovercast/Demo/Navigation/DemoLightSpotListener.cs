using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoLightSpotListener : DemoBaseListener<NavItemSticky> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.OnSelected += HandleSelected;
			Item.OnDeselected += HandleDeselected;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(NavItem pNavItem) {
			Enviro.ShowSpotlight(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleDeselected(NavItem pNavItem) {
			Enviro.ShowSpotlight(false);
		}

	}

}
