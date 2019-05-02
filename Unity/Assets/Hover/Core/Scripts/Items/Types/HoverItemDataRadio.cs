using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataRadio : HoverItemDataSelectableBool, IItemDataRadio {
		
		[SerializeField]
		private string _DefaultGroupId;

		[SerializeField]
		private string _GroupId;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void InitDefaultGroupId(Transform pParentTx) {
			if ( pParentTx == null ) {
				_DefaultGroupId = "Group-Root";
				return;
			}

			IItemData parentData = pParentTx.GetComponent<IItemData>();

			if ( parentData != null ) {
				_DefaultGroupId = "Group-"+parentData.AutoId;
				return;
			}

			_DefaultGroupId = "Group-Instance"+pParentTx.GetInstanceID();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string DefaultGroupId {
			get => _DefaultGroupId;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string GroupId {
			get => (string.IsNullOrEmpty(_GroupId) ? _DefaultGroupId : _GroupId);
			set => this.UpdateValueWithTreeMessage(ref _GroupId, value, "GroupId");
		}

		/*--------------------------------------------------------------------------------------------*/
		[ContextMenu("Radio Select")]
		public override void Select() {
			Value = true;
			base.Select();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool IgnoreSelection {
			get => (Value || base.IgnoreSelection);
		}

	}

}
