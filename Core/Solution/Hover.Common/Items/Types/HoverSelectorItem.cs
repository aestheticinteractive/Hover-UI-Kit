using UnityEngine;

namespace Hover.Common.Items.Types {

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
