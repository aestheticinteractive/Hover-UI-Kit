using System;
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
			get { return _AllowIdleDeselection; }
			set { _AllowIdleDeselection = value; }
		}

	}

}
