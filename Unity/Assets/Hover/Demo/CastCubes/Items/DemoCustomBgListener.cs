using System;
using Hover.Cast.Custom.Standard;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.CastCubes.Items {

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
			BgAlpha = Item.RangeValue;
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			BgAlpha = Item.RangeValue;

			ItemSett.UpdateAllSettings(x => UpdateSettingsWithBgAlpha((ItemVisualSettingsStandard)x));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static void UpdateSettingsWithBgAlpha(ItemVisualSettingsStandard pSettings) {
			Color c = pSettings.BackgroundColor;
			c.a = BgAlpha;
			pSettings.BackgroundColor = c;

			c = pSettings.SliderFillColor;
			c.a = 0.5f*BgAlpha;
			pSettings.SliderFillColor = c;
		}

	}

}
