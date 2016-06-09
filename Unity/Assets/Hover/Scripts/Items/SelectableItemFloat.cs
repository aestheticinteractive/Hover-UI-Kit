using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Items {

	/*================================================================================================*/
	[Serializable]
	public abstract class SelectableItemFloat : SelectableItem, ISelectableItem<float> {
		
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<float>> { }

		public ValueChangedEventHandler OnValueChangedEvent = new ValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<float> OnValueChanged;

		[SerializeField]
		protected float _Value = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected SelectableItemFloat() {
			OnValueChanged += (x => { OnValueChangedEvent.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual float Value {
			get {
				return _Value;
			}
			set {
				if ( value == _Value ) {
					return;
				}

				_Value = value;
				OnValueChanged(this);
			}
		}

	}

}
