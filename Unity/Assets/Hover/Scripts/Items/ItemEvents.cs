namespace Hover.Items {

	/*================================================================================================*/
	public static class ItemEvents {

		public delegate void IsEnabledChangedHandler(IBaseItemData pItem);
		public delegate void IsVisibleChangedHandler(IBaseItemData pItem);

		public delegate void SelectedHandler(ISelectableItemData pItem);
		public delegate void DeselectedHandler(ISelectableItemData pItem);

		public delegate void ValueChangedHandler<T>(ISelectableItemData<T> pItem);

	}

}
