using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItem<T> : HoverSelectableItem {

		public new ISelectableItem<T> Item { get; private set; }

		public T Value;
		public UnityEvent<ISelectableItem<T>> OnValueChanged;

		protected readonly ValueBinder<T> vBindValue;
		private SelectableItem<T> vFullItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItem() {
			vBindValue = new ValueBinder<T>(
				(x => { Item.Value = x; }),
				(x => { Value = x; }),
				vFullItem.AreValuesEqual
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void Init(SelectableItem<T> pItem) {
			base.Init(pItem);
			Item = pItem;
			vFullItem = pItem;

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
