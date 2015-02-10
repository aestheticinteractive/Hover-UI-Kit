using System;
using Hovercast.Core.Navigation;
using Hovercast.Demo.Settings;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomSizeListener : DemoBaseListener<NavItemSlider> {


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
			DemoSettingsProvider.ArcSegmentSettings.TextSize = (int)Math.Round(Item.RangeValue);
		}

	}

}
