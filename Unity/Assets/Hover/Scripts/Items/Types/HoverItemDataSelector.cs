using System;

namespace Hover.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSelector : SelectableItem, ISelectorItem {
		
		public SelectorActionType _Action = SelectorActionType.Default;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SelectorActionType Action {
			get { return _Action; }
			set { _Action = value; }
		}

	}

}
