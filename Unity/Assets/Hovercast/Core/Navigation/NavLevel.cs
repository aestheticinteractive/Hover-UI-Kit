using System;
using System.Collections.Generic;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavLevel {

		public delegate void ItemSelectedHandler(NavLevel pNavLevel, NavItem pNavItem);
		public event ItemSelectedHandler OnItemSelected;

		public bool IsActive { get; private set; }
		public NavItemParent LastSelectedParentItem { get; private set; }

		private Func<NavItem[]> vGetItems;
		private NavItem[] vActiveItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavLevel(Func<NavItem[]> pGetItems) {
			vGetItems = pGetItems;
			OnItemSelected += ((l,i) => {});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] Items {
			get {
				if ( vActiveItems == null ) {
					vActiveItems = vGetItems();
				}
				
				return vActiveItems;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void SetActiveOnLevelChange(bool pIsActive, int pDirection) {
			if ( pIsActive == IsActive ) {
				return;
			}

			IsActive = pIsActive;
			vActiveItems = null;

			foreach ( NavItem item in Items ) {
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
			foreach ( NavItem item in Items ) {
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
