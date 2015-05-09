using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Common.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class LayoutState : IHoverboardLayoutState {

		public IItemLayout ItemLayout { get; private set; }
		public BaseItemState[] FullItems { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LayoutState(IItemLayout pItemLayout, InteractionSettings pSettings) {
			ItemLayout = pItemLayout;
			vSettings = pSettings;

			RefreshItems();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemState[] Items {
			get {
				return FullItems.Cast<IBaseItemState>().ToArray();
			}
		}



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RefreshItems() {
			FullItems = new BaseItemState[ItemLayout.Items.Length];

			for ( int i = 0 ; i < ItemLayout.Items.Length ; i++ ) {
				var item = new BaseItemState(ItemLayout.Items[i], vSettings);
				FullItems[i] = item;
			}
		}

	}

}
