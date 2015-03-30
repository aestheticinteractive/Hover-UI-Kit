using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public class DemoLightSpotListener : DemoBaseListener<IStickyItem> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnSelected += HandleSelected;
			Item.OnDeselected += HandleDeselected;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//do nothing...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(ISelectableItem pItem) {
			Enviro.ShowSpotlight(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleDeselected(ISelectableItem pItem) {
			Enviro.ShowSpotlight(false);
		}

	}

}
