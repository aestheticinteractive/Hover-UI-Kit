using System;
using Hover.Core.Utils;
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
			get => _Action;
			set => this.UpdateValueWithTreeMessage(ref _Action, value, "Action");
		}

	}

}
