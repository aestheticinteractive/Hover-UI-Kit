using System;
using UnityEngine.Events;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public abstract class HoverItemDataSelectable : HoverItemData, IItemDataSelectable {
		
		[Serializable]
		public class SelectedEventHandler : UnityEvent<IItemDataSelectable> {}
		
		public bool IsStickySelected { get; private set; }
		public virtual bool AllowIdleDeselection { get; set; }

		public SelectedEventHandler OnSelectedEvent = new SelectedEventHandler();
		public SelectedEventHandler OnDeselectedEvent = new SelectedEventHandler();

		public event ItemEvents.SelectedHandler OnSelected;
		public event ItemEvents.DeselectedHandler OnDeselected;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemDataSelectable() {
			OnSelected += (x => { OnSelectedEvent.Invoke(x); });
			OnDeselected += (x => { OnDeselectedEvent.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Select() {
			IsStickySelected = UsesStickySelection();
			OnSelected(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void DeselectStickySelections() {
			if ( !IsStickySelected ) {
				return;
			}

			IsStickySelected = false;
			OnDeselected(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IgnoreSelection {
			get {
				return false;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool UsesStickySelection() {
			return false;
		}

	}

}
