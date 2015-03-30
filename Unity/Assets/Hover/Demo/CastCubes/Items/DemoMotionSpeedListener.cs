using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public class DemoMotionSpeedListener : DemoBaseListener<ISliderItem> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.ValueToLabel = (s => Component.Label+": "+s.RangeValue.ToString("0.0")+"x");
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			Enviro.SetMotionSpeed(Item.RangeValue);
		}

	}

}
