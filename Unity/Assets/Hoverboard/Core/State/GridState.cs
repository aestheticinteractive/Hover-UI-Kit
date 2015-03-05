using Hoverboard.Core.Custom;
using Hoverboard.Core.Navigation;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class GridState {

		public NavGrid NavGrid { get; private set; }
		public ButtonState[] Buttons { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public GridState(NavGrid pNavGrid, InteractionSettings pSettings) {
			NavGrid = pNavGrid;
			vSettings = pSettings;

			RefreshButtons();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshButtons() {
			Buttons = new ButtonState[NavGrid.Items.Length];

			for ( int i = 0 ; i < NavGrid.Items.Length ; i++ ) {
				var button = new ButtonState(NavGrid.Items[i], vSettings);
				Buttons[i] = button;
			}
		}

	}

}
