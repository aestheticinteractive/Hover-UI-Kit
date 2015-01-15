using System;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavItem {

		public enum ItemType {
			Parent,
			Selection,
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
		public string Label { get; private set; }
		public float RelativeSize { get; private set; }
		public NavItem[] Children { get; private set; }

		public bool NavigateBackUponSelect { get; set; }

		private bool vIsSelected;
		private bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem(ItemType pType, string pLabel, float pRelativeSize=1) {
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
		public bool IsSelected {
			get {
				return vIsSelected;
			}
			set {
				if ( value && !vIsSelected ) {
					vIsSelected = true;
					OnSelected(this);
				}

				if ( !value && vIsSelected ) {
					vIsSelected = false;
					OnDeselected(this);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool UsesStickySelection() {
			return (Type == ItemType.Sticky || Type == ItemType.Slider);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsStickySelected() {
			return (IsSelected && UsesStickySelection());
		}

	}

}
