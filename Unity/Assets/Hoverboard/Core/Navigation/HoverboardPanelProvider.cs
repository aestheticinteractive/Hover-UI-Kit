using System.Collections.Generic;
using UnityEngine;

namespace Hoverboard.Core.Navigation {

	/*================================================================================================*/
	public class HoverboardPanelProvider : MonoBehaviour {

		private NavPanel vPanel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavPanel GetPanel() {
			if ( vPanel == null ) {
				vPanel = new NavPanel(GetChildGrids, gameObject);
			}

			return vPanel;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private NavGrid[] GetChildGrids() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var grids = new List<NavGrid>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardGridProvider hgi = tx.GetChild(i).GetComponent<HoverboardGridProvider>();
				NavGrid grid = hgi.GetGrid();

				if ( !grid.IsVisible ) {
					continue;
				}

				grids.Add(grid);
			}

			return grids.ToArray();
		}

	}

}
