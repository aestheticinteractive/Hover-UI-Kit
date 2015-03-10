using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Navigation;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display {

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
				NavGrid navGrid = grid.NavGrid;
				var pos = new Vector3(navGrid.ColOffset, 0, navGrid.RowOffset);

				UiGrid uiGrid = navGrid.Container.AddComponent<UiGrid>();
				uiGrid.Build(grid, pCustom);
				uiGrid.transform.localPosition = pos*UiButton.Size;

				vUiGrids.Add(uiGrid);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
		}

	}

}
