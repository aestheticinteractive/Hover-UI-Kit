using System;
using UnityEngine;

namespace Hoverboard.Core.Navigation {

	/*================================================================================================*/
	public class NavPanel { 

		public delegate void ItemSelectionHandler(NavGrid pGrid, NavItem pItem);

		public event ItemSelectionHandler OnItemSelection;

		public bool IsVisible { get; private set; }
		public GameObject Container { get; private set; }

		private readonly Func<NavGrid[]> vGetGrids;
		private NavGrid[] vActiveGrids;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavPanel(Func<NavGrid[]> pGetGrids, GameObject pContainer) {
			vGetGrids = pGetGrids;
			Container = pContainer;

			OnItemSelection += ((l,i) => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavGrid[] Grids {
			get {
				if ( vActiveGrids == null ) {
					vActiveGrids = vGetGrids();
				}

				return vActiveGrids;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void SetVisible(bool pIsVisible) {
			if ( pIsVisible == IsVisible ) {
				return;
			}

			IsVisible = pIsVisible;
			vActiveGrids = null;

			foreach ( NavGrid grid in Grids ) {
				grid.SetActive(pIsVisible);

				if ( IsVisible ) {
					grid.OnItemSelected += HandleItemSelected;
				}
				else {
					grid.OnItemSelected -= HandleItemSelected;
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(NavGrid pGrid, NavItem pItem) {
			OnItemSelection(pGrid, pItem);
		}

	}

}
