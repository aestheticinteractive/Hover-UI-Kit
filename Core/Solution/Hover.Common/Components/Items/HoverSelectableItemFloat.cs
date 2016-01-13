using System;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;
using UnityEngine;

namespace Hover.Common.Components.Items {
	
	/*================================================================================================*/
	public abstract class HoverSelectableItemFloat : HoverSelectableItem {
		
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<float>> {}
		
		public new ISelectableItem<float> Item { get; private set; }
		
		public float Value;
		public ValueChangedEventHandler OnValueChanged;
		
		protected readonly ValueBinder<float> vBindValue;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItemFloat() {
			vBindValue = new ValueBinder<float>(
				(x => { Item.Value = x; }),
				(x => { Value = x; }),
				ValueBinder.AreFloatsEqual
			);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void Init(SelectableItem<float> pItem) {
			base.Init(pItem);
			Item = pItem;
			
			Item.OnValueChanged += (x => {
				if ( OnValueChanged != null ) {
					OnValueChanged.Invoke(x);
				}
			});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);
			vBindValue.UpdateValuesIfChanged(Item.Value, Value, pForceUpdate);
		}
		
	}
	
}
