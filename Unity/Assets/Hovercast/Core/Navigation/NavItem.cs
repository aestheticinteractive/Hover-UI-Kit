using System;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public abstract class NavItem : MonoBehaviour {

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

		public string BaseLabel = "";
		public float RelativeSize = 1;

		public int Id { get; private set; }
		public abstract ItemType Type { get; }
		public NavLevel ChildLevel { get; protected set; }

		public bool IsStickySelected { get; private set; }
		public virtual bool NavigateBackUponSelect { get; set; }

		protected bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			Id = (++ItemCount);
			vIsEnabled = true;

			OnSelected += (i => {});
			OnDeselected += (i => {});
			OnEnabled += (i => {});
			OnDisabled += (i => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get {
				return (BaseLabel ?? gameObject.name);
			}
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
		public override void Awake() {
			base.Awake();
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
