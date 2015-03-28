namespace Hover.Demo.CastCubes.Navigation {

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
			InteractSett.IsMenuOnLeftSide = !InteractSett.IsMenuOnLeftSide;
		}

	}

}
