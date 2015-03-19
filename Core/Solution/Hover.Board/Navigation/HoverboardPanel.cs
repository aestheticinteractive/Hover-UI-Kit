using System.Collections.Generic;
using UnityEngine;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public class HoverboardPanel : MonoBehaviour {

		private ItemPanel vPanel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemPanel GetPanel() {
			if ( vPanel == null ) {
				vPanel = new ItemPanel(GetChildGrids);
				vPanel.DisplayContainer = gameObject;
			}

			return vPanel;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemGrid[] GetChildGrids() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var grids = new List<IItemGrid>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardGrid hgi = tx.GetChild(i).GetComponent<HoverboardGrid>();
				IItemGrid grid = hgi.GetGrid();

				/*if ( !grid.IsVisible ) {
					continue;
				}*/

				grids.Add(grid);
			}

			return grids.ToArray();
		}

	}

}
