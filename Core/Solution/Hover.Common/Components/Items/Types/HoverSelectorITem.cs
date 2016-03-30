using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSelectorItem : HoverSelectableItem, ISelectorItem {

		[SerializeField]
		private bool vNavigateBackUponSelect;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool NavigateBackUponSelect {
			get { return vNavigateBackUponSelect; }
			set { vNavigateBackUponSelect = value; }
		}

	}

}
