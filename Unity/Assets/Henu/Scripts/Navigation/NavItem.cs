using System;

namespace Henu.Navigation {

	/*================================================================================================*/
	public abstract class NavItem {

		public enum ItemType {
			Parent,
			Selector,
			Sticky,
			Checkbox,
			Radio,
			Slider
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

		public int Id { get; private set; }
		public ItemType Type { get; private set; }
		public virtual string Label { get; private set; }
		public float RelativeSize { get; private set; }
		public NavItem[] Children { get; private set; }

		public bool IsStickySelected { get; private set; }
		public bool NavigateBackUponSelect { get; set; }

		protected bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected NavItem(ItemType pType, string pLabel, float pRelativeSize=1) {
			Id = (++ItemCount);
			Type = pType;
			Label = (pLabel ?? "");
			RelativeSize = pRelativeSize;
			Children = null;
			vIsEnabled = true;

			OnSelected += (i => {});
			OnDeselected += (i => {});
			OnEnabled += (i => {});
			OnDisabled += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetChildren(NavItem[] pChildren) {
			if ( Children != null ) {
				throw new Exception("Children already set.");
			}

			if ( Type != ItemType.Parent ) {
				throw new Exception("Only items of type 'Parent' can have children.");
			}

			Children = pChildren;
		}

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
		internal virtual void UpdateValueOnLevelChange(int pDirection) {
			IsStickySelected = false;
		}

	}


	/*================================================================================================*/
	public abstract class NavItem<T> : NavItem where T : IComparable {

		public delegate void ValueChangedHandler(NavItem<T> pNavItem);
		public event ValueChangedHandler OnValueChanged;

		protected T vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected NavItem(ItemType pType, string pLabel, float pRelativeSize=1) : 
																	base(pType, pLabel, pRelativeSize) {
			OnValueChanged += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual T Value {
			get {
				return vValue;
			}
			set {
				if ( value.CompareTo(vValue) == 0 ) {
					return;
				}

				vValue = value;
				OnValueChanged(this);
			}
		}

	}

}
