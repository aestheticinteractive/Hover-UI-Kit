using UnityEngine;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public abstract class NavItem {

		public enum ItemType {
			Selector,
			Sticky
		}

		public delegate void SelectedHandler(NavItem pNavItem);
		public delegate void DeselectedHandler(NavItem pNavItem);
		public delegate void EnabledHandler(NavItem pNavItem);
		public delegate void DisabledHandler(NavItem pNavItem);

		public event SelectedHandler OnSelected;
		public event DeselectedHandler OnDeselected;
		public event EnabledHandler OnEnabled;
		public event DisabledHandler OnDisabled;

		private static int ItemCount;

		public GameObject Container { get; internal set; }
		public int AutoId { get; private set; }
		public string Id { get; internal set; }
		public ItemType Type { get; private set; }
		public virtual string Label { get; internal set; }
		public int Width { get; internal set; }
		public bool IsVisible { get; set; }
		public bool IsStickySelected { get; private set; }

		protected bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem(ItemType pType) {
			AutoId = (++ItemCount);
			Id = "NavItem"+AutoId;
			Type = pType;
			vIsEnabled = true;

			OnSelected += (i => {});
			OnDeselected += (i => {});
			OnEnabled += (i => {});
			OnDisabled += (i => {});
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
		public virtual bool IsEnabled {
			get {
				return vIsEnabled;
			}
			set {
				if ( value && !vIsEnabled ) {
					vIsEnabled = true;
					OnEnabled(this);
				}

				if ( !value && vIsEnabled ) {
					vIsEnabled = false;
					OnDisabled(this);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool AllowSelection {
			get {
				return vIsEnabled;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool UsesStickySelection() {
			return false;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal virtual void UpdateValueOnActiveChange(bool pIsActive) {
			IsStickySelected = false;
		}

	}

}
