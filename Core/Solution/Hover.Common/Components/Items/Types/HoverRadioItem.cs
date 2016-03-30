using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverRadioItem : HoverSelectableItemBool, IRadioItem {

		[SerializeField]
		private string vGroupId;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GroupId { //TODO: doesn't update for runtime changes
			get { return vGroupId; }
			set { vGroupId = value; }
		}

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
