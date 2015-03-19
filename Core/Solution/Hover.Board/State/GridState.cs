using Hover.Board.Items;
using Hover.Common.Custom;
using Hover.Common.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class GridState {

		public IItemGrid ItemGrid { get; private set; }
		public BaseItemState[] Items { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public GridState(IItemGrid pItemGrid, InteractionSettings pSettings) {
			ItemGrid = pItemGrid;
			vSettings = pSettings;

			RefreshItems();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshItems() {
			Items = new BaseItemState[ItemGrid.Items.Length];

			for ( int i = 0 ; i < ItemGrid.Items.Length ; i++ ) {
				var item = new BaseItemState(ItemGrid.Items[i], vSettings);
				Items[i] = item;
			}
		}

	}

}
