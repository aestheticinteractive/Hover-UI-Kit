using System.Diagnostics;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoColorHuePlusListener : DemoBaseListener<NavItemSticky> {

		public HovercastNavItem HueSlider;
		public float Increment = 1f;
		public int IncrementFrequency = 500;

		private NavItemSlider vSliderItem;
		private float vRawIncrement;
		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnSelected += HandleSelected;
			Item.OnDeselected += HandleDeselected;

			vSliderItem = (NavItemSlider)HueSlider.GetItem();
			vRawIncrement = Increment/(vSliderItem.RangeMax-vSliderItem.RangeMin);
			vTimer = new Stopwatch();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vTimer.ElapsedMilliseconds >= IncrementFrequency ) {
				HandleSelected(null);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(NavItem pNavItem) {
			vSliderItem.Value += vRawIncrement;
			vTimer.Reset();
			vTimer.Start();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleDeselected(NavItem pNavItem) {
			vTimer.Stop();
		}

	}

}
