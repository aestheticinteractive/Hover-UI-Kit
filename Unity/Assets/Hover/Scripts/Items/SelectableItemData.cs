using System;
using UnityEngine.Events;

namespace Hover.Items {

	/*================================================================================================*/
	[Serializable]
	public abstract class SelectableItemData : HoverItemData, ISelectableItemData {
		
		[Serializable]
		public class SelectedEventHandler : UnityEvent<ISelectableItemData> {}
		
		public bool IsStickySelected { get; private set; }

		public SelectedEventHandler OnSelectedEvent = new SelectedEventHandler();
		public SelectedEventHandler OnDeselectedEvent = new SelectedEventHandler();

		public event ItemEvents.SelectedHandler OnSelected;
		public event ItemEvents.DeselectedHandler OnDeselected;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected SelectableItemData() {
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
		public virtual bool AllowSelection {
			get {
				return IsEnabled;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool UsesStickySelection() {
			return false;
		}

	}

}
