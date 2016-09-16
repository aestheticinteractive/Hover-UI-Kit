using System;
using UnityEngine;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSelector : HoverItemDataSelectable, IItemDataSelector {

		[SerializeField]
		private SelectorActionType _Action = SelectorActionType.Default;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SelectorActionType Action {
			get { return _Action; }
			set { _Action = value; }
		}

	}

}
