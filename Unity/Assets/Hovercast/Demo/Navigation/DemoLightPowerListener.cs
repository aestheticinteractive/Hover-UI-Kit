using System;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoLightPowerListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.ValueToLabel = (s => Component.Label+": "+Math.Round(s.RangeValue*100));
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Enviro.SetLightIntensitiy(Item.RangeValue);
		}

	}

}
