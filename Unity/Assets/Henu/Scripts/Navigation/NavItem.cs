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

		public delegate void SelectionHandler(NavItem pNavItem);
		public delegate void DeselectionHandler(NavItem pNavItem);

		public event SelectionHandler OnSelection;
		public event DeselectionHandler OnDeselection;

		private static int ItemCount;

		public int Id { get; private set; }
		public ItemType Type { get; private set; }
		public string Label { get; private set; }
		public float RelativeSize { get; private set; }
		public NavItem[] Children { get; private set; }

		public bool Selected { get; private set; }
		public bool NavigateBackUponSelect { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem(ItemType pType, string pLabel, float pRelativeSize=1) {
			Id = (++ItemCount);
			Type = pType;
			Label = (pLabel ?? "");
			RelativeSize = pRelativeSize;
			Children = null;

			OnSelection += (i => {});
			OnDeselection += (i => {});
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
		public void Select() {
			if ( Selected ) {
				return;
			}

			Selected = true;
			OnSelection(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Deselect() {
			if ( !Selected ) {
				return;
			}

			Selected = false;
			OnDeselection(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ToggleSelect() {
			if ( Selected ) {
				Deselect();
			}
			else {
				Select();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool UsesStickySelection() {
			return (Type == ItemType.Sticky || Type == ItemType.Slider);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsStickySelected() {
			return (Selected && UsesStickySelection());
		}

	}

}
