namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class SelectableItem : BaseItem, ISelectableItem {

		public event ItemEvents.SelectedHandler OnSelected;
		public event ItemEvents.DeselectedHandler OnDeselected;

		public bool IsStickySelected { get; private set; }
		public virtual bool NavigateBackUponSelect { get; set; }
		//public float RelativeSize { get; internal set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected SelectableItem() {
			OnSelected += (i => {});
			OnDeselected += (i => {});
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
