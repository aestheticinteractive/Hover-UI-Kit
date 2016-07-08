namespace Hover.Items {

	/*================================================================================================*/
	public interface ISelectableItemData<T> : ISelectableItemData {

		event ItemEvents.ValueChangedHandler<T> OnValueChanged;

		T Value { get; set; }

	}

}
