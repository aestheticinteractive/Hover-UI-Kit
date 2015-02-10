using Hovercast.Core.Custom;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Demo.Custom {

	/*================================================================================================*/
	public class DemoHueSegment : HovercastDefaultSegment {

		private SegmentSettings vSettings;
		private NavItemSlider vHueSlider;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();

			vSettings = GetSettings();
			
			vHueSlider = (NavItemSlider)gameObject.GetComponent<HovercastNavItem>().GetItem();
			vHueSlider.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Color col = DemoEnvironment.HsvToColor(vHueSlider.RangeValue, 1, 0.666f);

			col.a = 1;
			vSettings.SelectionColor = col;

			col.a = 0.25f;
			vSettings.SliderTrackColor = col;
			vSettings.SliderFillColor = col;
		}

	}

}
