using System;
using UnityEngine;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class RadioItem : SelectableItemBool, IRadioItem {
		
		[SerializeField]
		private string vDefaultGroupId;

		[SerializeField]
		private string vGroupId;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void InitDefaultGroupId(Transform pParentTx) {
			if ( pParentTx == null ) {
				vDefaultGroupId = "Group-Root";
				return;
			}
			
			HoverItemData parentData = pParentTx.GetComponent<HoverItemData>();

			if ( parentData != null ) {
				vDefaultGroupId = "Group-"+parentData.Data.AutoId;
				return;
			}

			vDefaultGroupId = "Group-Instance"+pParentTx.GetInstanceID();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GroupId { //TODO: doesn't update for runtime changes
			get { return (string.IsNullOrEmpty(vGroupId) ? vDefaultGroupId : vGroupId); }
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

	}

}
