using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class PanelState {

		public ItemPanel ItemPanel { get; private set; }
		public LayoutState[] Layouts { get; private set; }
		public PlaneData InteractionPlane { get; set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PanelState(ItemPanel pItemPanel, InteractionSettings pSettings) {
			ItemPanel = pItemPanel;
			vSettings = pSettings;

			RefreshLayouts();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshLayouts() {
			Layouts = new LayoutState[ItemPanel.Layouts.Length];

			for ( int i = 0 ; i < ItemPanel.Layouts.Length ; i++ ) {
				var layout = new LayoutState(ItemPanel.Layouts[i], vSettings);
				Layouts[i] = layout;
			}
		}

	}

}
