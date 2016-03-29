using System;
using Hover.Common.Items;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItemFloat : HoverSelectableItem, ISelectableItem<float> {

		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<float>> { }

		public ValueChangedEventHandler _OnValueChanged = new ValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<float> OnValueChanged;

		protected float vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItemFloat() {
			OnValueChanged += (x => { _OnValueChanged.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual float Value {
			get {
				return vValue;
			}
			set {
				if ( value == vValue ) {
					return;
				}

				vValue = value;
				OnValueChanged(this);
			}
		}

	}

}
