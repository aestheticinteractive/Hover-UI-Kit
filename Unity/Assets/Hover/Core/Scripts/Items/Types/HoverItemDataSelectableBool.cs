using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public abstract class HoverItemDataSelectableBool : 
													HoverItemDataSelectable, IItemDataSelectable<bool> {
		
		[Serializable]
		public class BoolValueChangedEventHandler : UnityEvent<IItemDataSelectable<bool>> {}
		
		public BoolValueChangedEventHandler OnValueChangedEvent = new BoolValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<bool> OnValueChanged;

		[SerializeField]
		protected bool _Value = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemDataSelectableBool() {
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
