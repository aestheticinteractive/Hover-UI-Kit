using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Util;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverRadioItem : HoverSelectableItemBool {

		public new IRadioItem Item { get; private set; }

		public string GroupId;

		private readonly ValueBinder<string> vBindGroup;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRadioItem() {
			Item = new RadioItem();
			Init((SelectableItem<bool>)Item);

			vBindGroup = new ValueBinder<string>(
				(x => { Item.GroupId = x; }),
				(x => { GroupId = x; }),
				ValueBinder.AreStringsEqual
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			
			if ( string.IsNullOrEmpty(GroupId) ) {
				GroupId = gameObject.transform.parent.gameObject.name;
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);
			vBindGroup.UpdateValuesIfChanged(Item.GroupId, GroupId, pForceUpdate);
		}

	}

}
