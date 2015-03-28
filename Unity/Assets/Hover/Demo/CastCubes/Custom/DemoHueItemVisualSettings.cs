using Hover.Cast;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.Items;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.CastCubes.Custom {

	/*================================================================================================*/
	public class DemoHueItemVisualSettings : HovercastItemVisualSettings {

		private IItemVisualSettings vRootSettings;
		private ItemVisualSettingsStandard vHueSettings;
		private ISliderItem vHueSlider;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vRootSettings = GameObject.Find("Hovercast")
				.GetComponent<HovercastSetup>()
				.DefaultItemVisualSettings
				.GetSettings(new SliderItem());

			vHueSettings = new ItemVisualSettingsStandard();
			
			vHueSlider = (ISliderItem)gameObject.GetComponent<HovercastItem>().GetItem();
			vHueSlider.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override IItemVisualSettings GetSettingsInner(IBaseItem pItem) {
			HandleValueChanged(null);
			return vHueSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem pItem) {
			Color col = DemoEnvironment.HsvToColor(vHueSlider.RangeValue, 1, 0.666f);

			Color colFade = col;
			colFade.a = 0.25f;

			ItemVisualSettingsStandard.Fill((ItemVisualSettingsStandard)vRootSettings, vHueSettings);
			vHueSettings.SelectionColor = col;
			vHueSettings.SliderTrackColor = colFade;
			vHueSettings.SliderFillColor = colFade;
		}

	}

}
