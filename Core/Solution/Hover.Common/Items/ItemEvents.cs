using Hover.Common.Items.Groups;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public static class ItemEvents {

		public delegate void IsEnabledChangedHandler(IBaseItem pItem);
		public delegate void IsVisibleChangedHandler(IBaseItem pItem);

		public delegate void SelectedHandler(ISelectableItem pItem);
		public delegate void DeselectedHandler(ISelectableItem pItem);

		public delegate void ValueChangedHandler<T>(ISelectableItem<T> pItem);

		public delegate void GroupItemSelectedHandler(IItemGroup pGroup, ISelectableItem pItem);
		public delegate void HierarchyLevelChangedHandler(int pDirection);

	}

}
