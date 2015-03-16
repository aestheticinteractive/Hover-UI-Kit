using System;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface ISelectableItem<T> : ISelectableItem where T : IComparable {

		event ItemEvents.ValueChangedHandler<T> OnValueChanged;

		T Value { get; set; }

	}

}
