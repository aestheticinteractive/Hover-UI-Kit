using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public class DemoLightPosListener : DemoBaseListener<ISliderItem> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();

			Item.ValueToLabel = (s => {
				string lbl = "";

				switch ( (int)Math.Round(s.SnappedValue*(s.Snaps-1)) ) {
					case 0: lbl = "Lowest"; break;
					case 1: lbl = "Low"; break;
					case 2: lbl = "High"; break;
					case 3: lbl = "Highest"; break;
				}

				return Component.Label+": "+lbl;
			});

			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			Enviro.SetLightPos(Item.RangeValue);
		}

	}

}
