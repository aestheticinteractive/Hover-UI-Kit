using System;
using UnityEngine;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class SelectorItem : SelectableItem, ISelectorItem {
		
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
