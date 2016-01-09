using Hover.Common.Components.Items;
using Hover.Common.Util;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class HoverSelectableItem : HoverBaseItem, ISelectableItem {

		public event ItemEvents.SelectedHandler OnSelected;
		public event ItemEvents.DeselectedHandler OnDeselected;

		//TODO: add "UnityEvent" fields to set actions directly via the editor

		private bool _IsStickySelected;
		private bool _AllowSelection;
		public bool _NavigateBackUponSelect;

		private readonly SelectableItem vCoreItem;
		private readonly ValueBinder<bool> vBindBack;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSelectableItem(SelectableItem pCoreItem) : base(pCoreItem) {
			vCoreItem = pCoreItem;

			OnSelected += (i => {});
			OnDeselected += (i => {});

			vCoreItem.OnSelected += OnSelected; //TODO: is this valid?
			vCoreItem.OnDeselected += OnDeselected;

			vBindBack = new ValueBinder<bool>(
				(x => { vCoreItem.NavigateBackUponSelect = x; }),
				(x => { _NavigateBackUponSelect = x; }),
				ValueBinder.AreBoolsEqual
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsStickySelected {
			get {
				return vCoreItem.IsStickySelected;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool AllowSelection {
			get {
				return vCoreItem.AllowSelection;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool NavigateBackUponSelect {
			get {
				return vCoreItem.NavigateBackUponSelect;
			}
			set {
				vBindBack.UpdateValuesIfChanged(value);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Select() {
			vCoreItem.Select();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void DeselectStickySelections() {
			vCoreItem.DeselectStickySelections();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);

			_IsStickySelected = vCoreItem.IsStickySelected;
			_AllowSelection = vCoreItem.AllowSelection;
			vBindBack.UpdateValuesIfChanged(_NavigateBackUponSelect, pForceUpdate);
		}

	}

}
