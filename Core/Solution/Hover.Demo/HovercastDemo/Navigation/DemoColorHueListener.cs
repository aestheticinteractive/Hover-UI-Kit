using System;

namespace Hover.Demo.HovercastDemo.Navigation {

	/*================================================================================================*/
	public class DemoColorHueListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.ValueToLabel = (s => Component.Label+": "+Math.Round(s.RangeValue));
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Enviro.SetColorMode(Enviro.GetColorMode(), Item.RangeValue);
		}

	}

}
