using System;
using UnityEngine;
using System.Collections.Generic;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavLevel {

		public delegate void ItemSelectedHandler(NavLevel pNavLevel, NavItem pNavItem);
		public event ItemSelectedHandler OnItemSelected;

		public bool IsActive { get; private set; }
		public NavItemParent LastSelectedParentItem { get; private set; }

		private GameObject vParentObj;
		private NavItem[] vActiveItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavLevel() {
			OnItemSelected += ((l,i) => {});
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Build(GameObject pParentObj) {
			vParentObj = pParentObj;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItem[] Items {
			get {
				if ( IsActive && vActiveItems != null ) {
					return vActiveItems;
				}

				if ( vParentObj == null ) {
					vActiveItems = null;
					return new NavItem[0];
				}

				int childCount = vParentObj.transform.childCount;
				var items = new List<NavItem>();

				for ( int i = 0 ; i < childCount ; ++i ) {
					NavItem item = vParentObj.transform.GetChild(i).GetComponent<NavItem>();
					items.Add(item);
				}

				vActiveItems = items.ToArray();
				return vActiveItems;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void SetActiveOnLevelChange(bool pIsActive, int pDirection) {
			if ( pIsActive == IsActive ) {
				return;
			}

			IsActive = pIsActive;

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
