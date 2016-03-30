using System;
using Hover.Common.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {
	
	/*================================================================================================*/
	public abstract class HoverSelectableItemBool : HoverSelectableItem, ISelectableItem<bool> {
		
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<bool>> {}
		
		public ValueChangedEventHandler OnValueChangedEvent = new ValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<bool> OnValueChanged;

		[SerializeField]
		protected bool vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItemBool() {
			OnValueChanged += (x => { OnValueChangedEvent.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual bool Value {
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
