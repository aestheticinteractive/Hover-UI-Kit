using System;

namespace Hover.Demo.CastCubes.Navigation {

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
			//Don't automatically override the demo's default settings
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			SegSett.TextSize = (int)Math.Round(Item.RangeValue);
		}

	}

}
