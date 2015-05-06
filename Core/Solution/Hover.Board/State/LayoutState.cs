using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class LayoutState {

		public IItemLayout ItemLayout { get; private set; }
		public BaseItemState[] Items { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LayoutState(IItemLayout pItemLayout, InteractionSettings pSettings) {
			ItemLayout = pItemLayout;
			vSettings = pSettings;

			RefreshItems();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshItems() {
			Items = new BaseItemState[ItemLayout.Items.Length];

			for ( int i = 0 ; i < ItemLayout.Items.Length ; i++ ) {
				var item = new BaseItemState(ItemLayout.Items[i], vSettings);
				Items[i] = item;
			}
		}

	}

}
