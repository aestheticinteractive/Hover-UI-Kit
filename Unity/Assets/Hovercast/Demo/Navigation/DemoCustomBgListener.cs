using System;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomBgListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.ValueToLabel = (s => Component.Label+": "+Math.Round(s.RangeValue*100)+"%");
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			DemoSettingsComponent.BgAlpha = Item.RangeValue;
			DemoSettingsComponent.UpdateSettingsWithBgAlpha();
		}

	}

}
