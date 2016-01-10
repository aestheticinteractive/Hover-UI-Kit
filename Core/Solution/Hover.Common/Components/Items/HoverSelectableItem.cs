using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItem : HoverBaseItem {

		public new ISelectableItem Item { get; private set; }

		private bool IsStickySelected;
		private bool AllowSelection;
		public bool NavigateBackUponSelect;
		public UnityEvent<ISelectableItem> OnSelected;
		public UnityEvent<ISelectableItem> OnDeselected;

		private readonly ValueBinder<bool> vBindBack;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItem(SelectableItem pItem) : base(pItem) {
			Item = pItem;

			Item.OnSelected += (x => {
				if ( OnSelected != null ) {
					OnSelected.Invoke(x);
				}
			});

			Item.OnDeselected += (x => {
				if ( OnDeselected != null ) {
					OnDeselected.Invoke(x);
				}
			});

			vBindBack = new ValueBinder<bool>(
				(x => { Item.NavigateBackUponSelect = x; }),
				(x => { NavigateBackUponSelect = x; }),
				ValueBinder.AreBoolsEqual
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);

			IsStickySelected = Item.IsStickySelected;
			AllowSelection = Item.AllowSelection;
			vBindBack.UpdateValuesIfChanged(
				Item.NavigateBackUponSelect, NavigateBackUponSelect, pForceUpdate);
		}

	}

}
