using Hover.Board.Custom;
using Hover.Board.Items;
using Hover.Board.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiPanel : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(PanelState pPanelState, IItemVisualSettingsProvider pItemVisualSettProv) {
			for ( int i = 0 ; i < pPanelState.Grids.Length ; i++ ) {
				GridState grid = pPanelState.Grids[i];
				IItemGrid itemGrid = grid.ItemGrid;
				var pos = new Vector3(itemGrid.ColOffset, 0, itemGrid.RowOffset);
				GameObject gridObj = (GameObject)itemGrid.DisplayContainer;

				UiGrid uiGrid = gridObj.AddComponent<UiGrid>();
				uiGrid.Build(grid, pItemVisualSettProv);
				uiGrid.transform.localPosition = pos*UiItem.Size;
			}
		}

	}

}
