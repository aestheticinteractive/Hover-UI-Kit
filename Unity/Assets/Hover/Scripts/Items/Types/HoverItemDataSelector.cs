using System;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSelector : SelectableItemData, ISelectorItemData {
		
		public SelectorActionType _Action = SelectorActionType.Default;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SelectorActionType Action {
			get { return _Action; }
			set { _Action = value; }
		}

	}

}
