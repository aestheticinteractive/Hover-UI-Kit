using System.Collections.ObjectModel;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class LayoutState : IHoverboardLayoutState {

		public IItemLayout ItemLayout { get; private set; }
		public BaseItemState[] FullItems { get; private set; }
		public ReadOnlyCollection<IBaseItemState> Items { get; private set; }
		public float DisplayStrength { get; set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LayoutState(IItemLayout pItemLayout, InteractionSettings pSettings) {
			ItemLayout = pItemLayout;
			vSettings = pSettings;
			DisplayStrength = 1;
			FullItems = new BaseItemState[ItemLayout.Items.Length];

			for ( int i = 0 ; i < ItemLayout.Items.Length ; i++ ) {
				var item = new BaseItemState(ItemLayout.Items[i], vSettings);
				FullItems[i] = item;
			}

			Items = new ReadOnlyCollection<IBaseItemState>(FullItems);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PreventEveryItemSelectionViaDisplay(string pName, bool pPrevent) {
			foreach ( BaseItemState item in FullItems ) {
				item.PreventSelectionViaDisplay(pName, pPrevent);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEveryItemSelectionPreventedViaDisplay() {
			foreach ( BaseItemState item in FullItems ) {
				if ( !item.IsSelectionPreventedViaDisplay() ) {
					return false;
				}
			}

			return true;
		}

	}

}
