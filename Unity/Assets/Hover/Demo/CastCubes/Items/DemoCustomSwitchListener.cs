using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public class DemoCustomSwitchListener : DemoBaseListener<ISelectorItem> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnSelected += HandleSelected;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			//do nothing...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSelected(ISelectableItem pItem) {
			InteractSett.IsMenuOnLeftSide = !InteractSett.IsMenuOnLeftSide;
		}

	}

}
