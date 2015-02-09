using System;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomSizeListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.ValueToLabel = (s => Component.Label+": "+Math.Round(s.RangeValue));
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			DemoSettingsComponent.ArcSegmentSettings.TextSize = (int)Math.Round(Item.RangeValue);
		}

	}

}
