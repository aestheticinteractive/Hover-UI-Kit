namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	public interface IItemDataSelectable<T> : IItemDataSelectable {

		event ItemEvents.ValueChangedHandler<T> OnValueChanged;

		T Value { get; set; }

	}

}
