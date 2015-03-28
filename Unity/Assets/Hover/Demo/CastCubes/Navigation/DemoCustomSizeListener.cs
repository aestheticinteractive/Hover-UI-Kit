using System;
using Hover.Cast.Custom.Standard;
using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoCustomSizeListener : DemoBaseListener<ISliderItem> {


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
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			ItemVisualSettingsStandard sett = (ItemVisualSettingsStandard)ItemSett.GetSettings(pItem);
			sett.TextSize = (int)Math.Round(Item.RangeValue);
		}

	}

}
