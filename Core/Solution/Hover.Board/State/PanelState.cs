using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class PanelState : IHoverboardPanelState {

		public IItemPanel ItemPanel { get; private set; }
		public LayoutState[] FullLayouts { get; private set; }
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
		public IHoverboardLayoutState[] Layouts {
			get {
				return FullLayouts.Cast<IHoverboardLayoutState>().ToArray();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void PreventEveryItemSelectionViaDisplay(string pName, bool pPrevent) {
			foreach ( LayoutState layout in FullLayouts ) {
				layout.PreventEveryItemSelectionViaDisplay(pName, pPrevent);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEveryItemSelectionPreventedViaDisplay() {
			return FullLayouts.All(x => x.IsEveryItemSelectionPreventedViaDisplay());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshLayouts() {
			FullLayouts = new LayoutState[ItemPanel.Layouts.Length];

			for ( int i = 0 ; i < ItemPanel.Layouts.Length ; i++ ) {
				var layout = new LayoutState(ItemPanel.Layouts[i], vSettings);
				FullLayouts[i] = layout;
			}
		}

	}

}
