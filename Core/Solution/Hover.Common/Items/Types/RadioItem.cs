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
		public RadioItem(string pDefaultGroupId) {

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
