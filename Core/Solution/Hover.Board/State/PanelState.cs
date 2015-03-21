using Hover.Board.Custom;
using Hover.Board.Items;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class PanelState {

		public ItemPanel ItemPanel { get; private set; }
		public GridState[] Grids { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PanelState(ItemPanel pItemPanel, InteractionSettings pSettings) {
			ItemPanel = pItemPanel;
			vSettings = pSettings;

			RefreshGrids();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshGrids() {
			Grids = new GridState[ItemPanel.Grids.Length];

			for ( int i = 0 ; i < ItemPanel.Grids.Length ; i++ ) {
				var grid = new GridState(ItemPanel.Grids[i], vSettings);
				Grids[i] = grid;
			}
		}

	}

}
