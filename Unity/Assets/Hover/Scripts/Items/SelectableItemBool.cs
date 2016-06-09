using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Items {

	/*================================================================================================*/
	[Serializable]
	public abstract class SelectableItemBool : SelectableItem, ISelectableItem<bool> {
		
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<bool>> {}
		
		public ValueChangedEventHandler OnValueChangedEvent = new ValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<bool> OnValueChanged;

		[SerializeField]
		protected bool _Value = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected SelectableItemBool() {
			OnValueChanged += (x => { OnValueChangedEvent.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual bool Value {
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
