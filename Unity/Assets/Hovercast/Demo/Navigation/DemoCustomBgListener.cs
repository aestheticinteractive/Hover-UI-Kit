using System;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
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
			UpdateSettingsWithBgAlpha(ArcSegSett);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static void UpdateSettingsWithBgAlpha(ArcSegmentSettings pArcSegSett) {
			Color c = pArcSegSett.BackgroundColor;
			c.a = BgAlpha;
			pArcSegSett.BackgroundColor = c;

			c = pArcSegSett.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			pArcSegSett.SliderFillColor = c;
		}

	}

}
