using System;
using Hovercast.Core.Navigation;
using Hovercast.Demo.Settings;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomBgListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.ValueToLabel = (s => Component.Label+": "+Math.Round(s.RangeValue*100)+"%");
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//Don't automatically override the demo's default settings
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			DemoSettingsProvider.BgAlpha = Item.RangeValue;
			DemoSettingsProvider.UpdateSettingsWithBgAlpha();
		}

	}

}
