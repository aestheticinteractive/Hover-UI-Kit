using System;
using Hover.Cast.Custom;
using Hover.Cast.Display.Default;
using Hover.Cast.Navigation;
using UnityEngine;

namespace Hover.Demo.HovercastDemo.Custom {

	/*================================================================================================*/
	public class DemoHueSegment : HovercastCustomSegment {

		private SegmentSettings vRootSettings;
		private SegmentSettings vHueSettings;
		private NavItemSlider vHueSlider;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vRootSettings = GameObject.Find("DemoEnvironment/MenuData")
				.GetComponent<HovercastCustomizationProvider>()
				.GetSegmentSettings(null);

			vHueSettings = new SegmentSettings();
			
			vHueSlider = (NavItemSlider)gameObject.GetComponent<HovercastNavItem>().GetItem();
			vHueSlider.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererForNavItemTypeInner(NavItem.ItemType pNavItemType) {
			return typeof(UiSliderRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override SegmentSettings GetSettings() {
			HandleValueChanged(null);
			return vHueSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Color col = DemoEnvironment.HsvToColor(vHueSlider.RangeValue, 1, 0.666f);

			Color colFade = col;
			colFade.a = 0.25f;

			SegmentSettings.Fill(vRootSettings, vHueSettings);
			vHueSettings.SelectionColor = col;
			vHueSettings.SliderTrackColor = colFade;
			vHueSettings.SliderFillColor = colFade;
		}

	}

}
