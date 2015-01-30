using System;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavLevel {

		public delegate void ItemSelectedHandler(NavLevel pNavLevel, NavItem pNavItem);
		public event ItemSelectedHandler OnItemSelected;

		public bool IsActive { get; private set; }
		public NavItemParent LastSelectedParentItem { get; private set; }

		private NavItem[] vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavLevel(params NavItem[] pItems) {
			vItems = pItems;
			OnItemSelected += ((l,i) => {});
		}

		/*--------------------------------------------------------------------------------------------*/
		public NavLevel() : this(new NavItem[0]) {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] Items {
			get {
				return vItems;
			}
			set {
				if ( IsActive ) {
					throw new Exception("Cannot change the Items list while the level is active.");
				}

				vItems = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void SetActiveOnLevelChange(bool pIsActive, int pDirection) {
			if ( pIsActive == IsActive ) {
				return;
			}

			IsActive = pIsActive;

			foreach ( NavItem item in vItems ) {
				item.UpdateValueOnLevelChange(pDirection);

				if ( IsActive ) {
					item.OnSelected += HandleItemSelected;
				}
				else {
					item.OnSelected -= HandleItemSelected;
					item.DeselectStickySelections();
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void HandleItemSelected(NavItem pItem) {
			if ( pItem.Type == NavItem.ItemType.Parent ) {
				LastSelectedParentItem = (NavItemParent)pItem;
			}

			if ( pItem.Type == NavItem.ItemType.Radio ) {
				DeselectRadioSiblings(pItem);
			}

			OnItemSelected(this, pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DeselectRadioSiblings(NavItem pSelectedItem) {
			foreach ( NavItem item in vItems ) {
				if ( item == pSelectedItem ) {
					continue;
				}

				NavItemRadio radItem = (item as NavItemRadio);

				if ( radItem == null ) {
					continue;
				}

				radItem.Value = false;
			}
		}

	}

}
