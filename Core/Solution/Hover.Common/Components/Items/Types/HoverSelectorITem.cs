using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Util;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSelectorItem : HoverSelectableItem {

		public new ISelectorItem Item { get; private set; }

		public bool NavigateBackUponSelect;
		
		protected readonly ValueBinder<bool> vBindBack;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectorItem() {
			Item = new SelectorItem();
			Init((SelectableItem)Item);
			
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
			
			vBindBack.UpdateValuesIfChanged(
				Item.NavigateBackUponSelect, NavigateBackUponSelect, pForceUpdate);
		}

	}

}
