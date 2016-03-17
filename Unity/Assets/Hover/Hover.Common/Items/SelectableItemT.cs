namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class SelectableItem<T> : SelectableItem, ISelectableItem<T> {

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
				if ( AreValuesEqual(value, vValue) ) {
					return;
				}

				vValue = value;
				OnValueChanged(this);
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract bool AreValuesEqual(T pValueA, T pValueB);

	}

}
