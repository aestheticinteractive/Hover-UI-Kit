using Hover.Board.Custom;
using Hover.Board.Navigation;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class PanelState {

		public NavPanel NavPanel { get; private set; }
		public GridState[] Grids { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PanelState(NavPanel pNavPanel, InteractionSettings pSettings) {
			NavPanel = pNavPanel;
			vSettings = pSettings;

			RefreshGrids();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshGrids() {
			Grids = new GridState[NavPanel.Grids.Length];

			for ( int i = 0 ; i < NavPanel.Grids.Length ; i++ ) {
				var grid = new GridState(NavPanel.Grids[i], vSettings);
				Grids[i] = grid;
			}
		}

	}

}
