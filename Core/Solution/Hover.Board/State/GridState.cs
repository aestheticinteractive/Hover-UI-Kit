using Hover.Board.Custom;
using Hover.Board.Navigation;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class GridState {

		public IItemGrid ItemGrid { get; private set; }
		public ButtonState[] Buttons { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public GridState(IItemGrid pItemGrid, InteractionSettings pSettings) {
			ItemGrid = pItemGrid;
			vSettings = pSettings;

			RefreshButtons();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshButtons() {
			Buttons = new ButtonState[ItemGrid.Items.Length];

			for ( int i = 0 ; i < ItemGrid.Items.Length ; i++ ) {
				var button = new ButtonState(ItemGrid.Items[i], vSettings);
				Buttons[i] = button;
			}
		}

	}

}
