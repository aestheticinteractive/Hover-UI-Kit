using System;
using UnityEngine;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSelector : SelectableItem, ISelectorItem {
		
		[SerializeField]
		private bool _NavigateBackUponSelect;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool NavigateBackUponSelect {
			get { return _NavigateBackUponSelect; }
			set { _NavigateBackUponSelect = value; }
		}

	}

}
