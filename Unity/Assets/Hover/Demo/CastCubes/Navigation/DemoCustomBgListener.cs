using System;
using Hover.Cast.Custom.Standard;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoCustomBgListener : DemoBaseListener<ISliderItem> {

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
			BgAlpha = Item.RangeValue;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			BgAlpha = Item.RangeValue;
			UpdateSettingsWithBgAlpha((ItemVisualSettingsStandard)ItemSett.GetSettings(pItem));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static void UpdateSettingsWithBgAlpha(ItemVisualSettingsStandard pSegSett) {
			Color c = pSegSett.BackgroundColor;
			c.a = BgAlpha;
			pSegSett.BackgroundColor = c;

			c = pSegSett.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			pSegSett.SliderFillColor = c;
		}

	}

}
