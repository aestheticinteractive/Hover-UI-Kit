using System;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class SelectableItem<T> : SelectableItem, ISelectableItem<T> where T : IComparable {

		public event ItemEvents.ValueChangedHandler<T> OnValueChanged;

		protected T vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected SelectableItem() {
			OnValueChanged += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual T Value {
			get {
				return vValue;
			}
			set {
				if ( value.CompareTo(vValue) == 0 ) {
					return;
				}

				vValue = value;
				OnValueChanged(this);
			}
		}

	}

}
