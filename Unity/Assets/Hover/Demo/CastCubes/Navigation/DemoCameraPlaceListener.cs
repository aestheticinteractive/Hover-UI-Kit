namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoCameraPlaceListener : DemoBaseListener<NavItemRadio> {

		public DemoEnvironment.CameraPlacement Placement;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			if ( !pNavItem.Value ) {
				return;
			}

			Enviro.SetCameraPlacement(Placement);
		}

	}

}
