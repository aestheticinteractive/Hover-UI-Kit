using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public abstract class HoverItemDataSelectableFloat : 
												HoverItemDataSelectable, IItemDataSelectable<float> {
		
		[Serializable]
		public class FloatValueChangedEventHandler : UnityEvent<IItemDataSelectable<float>> { }

		public FloatValueChangedEventHandler OnValueChangedEvent = new FloatValueChangedEventHandler();

		public event ItemEvents.ValueChangedHandler<float> OnValueChanged;

		[SerializeField]
		protected float _Value = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemDataSelectableFloat() {
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
