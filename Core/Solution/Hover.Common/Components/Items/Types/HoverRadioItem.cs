using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverRadioItem : HoverSelectableItemBool, IRadioItem {

		public string GroupId { get; set; }  //TODO: doesn't update for runtime changes


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = true;
			base.Select();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool AllowSelection {
			get {
				return (!Value && base.AllowSelection);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( string.IsNullOrEmpty(GroupId) ) {
				GroupId = gameObject.transform.parent.gameObject.name;
			}
		}
		
	}

}
