using System;

namespace Hovercast.Core.Navigation {

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
		public NavLevel ChildLevel { get; protected set; }

		public bool IsStickySelected { get; private set; }
		public virtual bool NavigateBackUponSelect { get; set; }

		protected bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected NavItem(ItemType pType, string pLabel, float pRelativeSize=1) {
			Id = (++ItemCount);
			Type = pType;
			Label = (pLabel ?? "");
			RelativeSize = pRelativeSize;
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
