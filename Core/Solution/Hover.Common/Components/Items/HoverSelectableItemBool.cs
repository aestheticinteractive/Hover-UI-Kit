using System;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;
using UnityEngine;

namespace Hover.Common.Components.Items {
	
	/*================================================================================================*/
	public abstract class HoverSelectableItemBool : HoverSelectableItem {
		
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<ISelectableItem<bool>> {}
		
		public new ISelectableItem<bool> Item { get; private set; }

		public bool Value;
		public ValueChangedEventHandler OnValueChanged;

		protected readonly ValueBinder<bool> vBindValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItemBool() {
			vBindValue = new ValueBinder<bool>(
				(x => { Item.Value = x; }),
				(x => { Value = x; }),
				ValueBinder.AreBoolsEqual
			);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void Init(SelectableItem<bool> pItem) {
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
