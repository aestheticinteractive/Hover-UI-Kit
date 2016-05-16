using System;
using UnityEngine;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class SelectorItem : SelectableItem, ISelectorItem {
		
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
