using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoColorModeListener : DemoBaseListener<NavItemRadio> {

		public HovercastNavItem HueSlider;
		public DemoEnvironment.ColorMode Mode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.OnValueChanged += HandleValueChanged;
		}

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
