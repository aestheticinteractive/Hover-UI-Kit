using System.Collections.Generic;
using Hover.Board.Custom;
using Hover.Board.Navigation;
using Hover.Board.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiPanel : MonoBehaviour {

		private PanelState vPanelState;
		private IList<UiGrid> vUiGrids;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(PanelState pPanel, ICustomSegment pCustom) {
			vPanelState = pPanel;
			vUiGrids = new List<UiGrid>();

			for ( int i = 0 ; i < vPanelState.Grids.Length ; i++ ) {
				GridState grid = vPanelState.Grids[i];
				IItemGrid itemGrid = grid.ItemGrid;
				var pos = new Vector3(itemGrid.ColOffset, 0, itemGrid.RowOffset);
				GameObject gridObj = (GameObject)itemGrid.DisplayContainer;

				UiGrid uiGrid = gridObj.AddComponent<UiGrid>();
				uiGrid.Build(grid, pCustom);
				uiGrid.transform.localPosition = pos*UiItem.Size;

				vUiGrids.Add(uiGrid);
			}
		}

	}

}
