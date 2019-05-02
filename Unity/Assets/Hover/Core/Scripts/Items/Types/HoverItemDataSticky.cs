using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSticky : HoverItemDataSelectable, IItemDataSticky {

		[SerializeField]
		private bool _AllowIdleDeselection = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool AllowIdleDeselection {
			get => _AllowIdleDeselection;
			set => this.UpdateValueWithTreeMessage(
				ref _AllowIdleDeselection, value, "AllowIdleDeselection");
		}

	}

}
