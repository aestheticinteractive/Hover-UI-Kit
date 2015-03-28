using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoCameraPlaceListener : DemoBaseListener<IRadioItem> {

		public DemoEnvironment.CameraPlacement Placement;


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

			Enviro.SetCameraPlacement(Placement);
		}

	}

}
