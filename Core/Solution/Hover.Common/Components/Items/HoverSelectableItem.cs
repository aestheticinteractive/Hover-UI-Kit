using System;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine.Events;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItem : HoverBaseItem {

		[Serializable]
		public class SelectedEventHandler : UnityEvent<ISelectableItem> {}
		
		public new ISelectableItem Item { get; private set; }

		public SelectedEventHandler OnSelected;
		public SelectedEventHandler OnDeselected;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void Init(SelectableItem pItem) {
			base.Init(pItem);
			Item = pItem;

			Item.OnSelected += (x => {
				if ( OnSelected != null ) {
					OnSelected.Invoke(x);
				}
			});

			Item.OnDeselected += (x => {
				if ( OnDeselected != null ) {
					OnDeselected.Invoke(x);
				}
			});
		}

	}

}
