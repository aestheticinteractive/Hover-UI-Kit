using System;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItem : HoverBaseItem {

		[Serializable]
		public class SelectedEventHandler : UnityEvent<ISelectableItem> {}
		
		public new ISelectableItem Item { get; private set; }

		protected bool IsStickySelected;
		protected bool AllowSelection;
		public bool NavigateBackUponSelect = false;
		public SelectedEventHandler OnSelected;
		public SelectedEventHandler OnDeselected;

		protected readonly ValueBinder<bool> vBindBack;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItem() {
			vBindBack = new ValueBinder<bool>(
				(x => { Item.NavigateBackUponSelect = x; }),
				(x => { NavigateBackUponSelect = x; }),
				ValueBinder.AreBoolsEqual
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void Init(SelectableItem pItem) {
			base.Init(pItem);
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
