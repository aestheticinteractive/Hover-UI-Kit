using Hover.Cast.Items;
using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public class DemoColorModeListener : DemoBaseListener<IRadioItem> {

		public HovercastItem HueSlider;
		public DemoEnvironment.ColorMode Mode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<bool> pItem) {
			if ( !pItem.Value ) {
				return;
			}

			ISliderItem hue = (ISliderItem)HueSlider.GetItem();
			hue.IsEnabled = (Mode == DemoEnvironment.ColorMode.Custom);
			Enviro.SetColorMode(Mode, hue.RangeValue);
		}

	}

}
