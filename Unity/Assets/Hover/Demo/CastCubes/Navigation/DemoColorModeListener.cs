namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoColorModeListener : DemoBaseListener<NavItemRadio> {

		public HovercastNavItem HueSlider;
		public DemoEnvironment.ColorMode Mode;


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

			NavItemSlider hue = (NavItemSlider)HueSlider.GetItem();
			hue.IsEnabled = (Mode == DemoEnvironment.ColorMode.Custom);
			Enviro.SetColorMode(Mode, hue.RangeValue);
		}

	}

}
