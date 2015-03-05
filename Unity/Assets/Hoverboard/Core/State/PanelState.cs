using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;

namespace Hoverboard.Core.State {

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
