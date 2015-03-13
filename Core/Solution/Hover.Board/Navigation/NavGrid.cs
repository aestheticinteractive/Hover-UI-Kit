using System;
using UnityEngine;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public class NavGrid {

		public delegate void ItemSelectedHandler(NavGrid pNavGrid, NavItem pNavItem);
		public event ItemSelectedHandler OnItemSelected;

		public bool IsVisible { get; private set; }
		public GameObject Container { get; private set; }
		public int Cols { get; private set; }
		public float RowOffset { get; private set; }
		public float ColOffset { get; private set; }

		private readonly Func<NavItem[]> vGetItems;
		private NavItem[] vActiveItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavGrid(Func<NavItem[]> pGetItems, GameObject pContainer, bool pIsVisible, int pCols,
																float pRowOffset, float pColOffset) {
			vGetItems = pGetItems;

			IsVisible = pIsVisible;
			Container = pContainer;
			Cols = pCols;
			RowOffset = pRowOffset;
			ColOffset = pColOffset;

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
		internal void SetActive(bool pIsActive) {
			if ( pIsActive == IsVisible ) {
				return;
			}

			IsVisible = pIsActive;
			vActiveItems = null;

			foreach ( NavItem item in Items ) {
				item.UpdateValueOnActiveChange(pIsActive);

				if ( IsVisible ) {
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
		private void HandleItemSelected(NavItem pItem) {
			OnItemSelected(this, pItem);
		}

	}

}
