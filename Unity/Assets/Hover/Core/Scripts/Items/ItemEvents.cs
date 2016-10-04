using Hover.Core.Items.Types;

namespace Hover.Core.Items {

	/*================================================================================================*/
	public static class ItemEvents {

		public delegate void EnabledChangedHandler(IItemData pItem);

		public delegate void SelectedHandler(IItemDataSelectable pItem);
		public delegate void DeselectedHandler(IItemDataSelectable pItem);

		public delegate void ValueChangedHandler<T>(IItemDataSelectable<T> pItem);

	}

}
