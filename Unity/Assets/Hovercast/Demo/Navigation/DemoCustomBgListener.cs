using System;
using Hovercast.Core.Custom;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomBgListener : DemoBaseListener<NavItemSlider> {

		private static float BgAlpha;


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
			BgAlpha = Item.RangeValue;
			UpdateSettingsWithBgAlpha(SegSett);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static void UpdateSettingsWithBgAlpha(SegmentSettings pSegSett) {
			Color c = pSegSett.BackgroundColor;
			c.a = BgAlpha;
			pSegSett.BackgroundColor = c;

			c = pSegSett.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			pSegSett.SliderFillColor = c;
		}

	}

}
