namespace Hover.Demo.HovercastDemo.Navigation {

	/*================================================================================================*/
	public class DemoLightSpotListener : DemoBaseListener<NavItemSticky> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnSelected += HandleSelected;
			Item.OnDeselected += HandleDeselected;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//do nothing...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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
