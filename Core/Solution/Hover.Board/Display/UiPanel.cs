using Hover.Board.Items;
using Hover.Board.State;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiPanel : MonoBehaviour {

		private GridState[] vGridStates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiPanel() {
			vGridStates = new GridState[0];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(PanelState pPanelState, IItemVisualSettingsProvider pItemVisualSettProv) {
			vGridStates = pPanelState.Grids;

			foreach ( GridState gridState in vGridStates ) {
				IItemGrid itemGrid = gridState.ItemGrid;
				var pos = new Vector3(itemGrid.ColOffset, 0, itemGrid.RowOffset);
				GameObject gridObj = (GameObject)itemGrid.DisplayContainer;

				UiGrid uiGrid = gridObj.AddComponent<UiGrid>();
				uiGrid.Build(gridState, pItemVisualSettProv);
				uiGrid.transform.localPosition = pos*UiItem.Size;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( GridState gridState in vGridStates ) {
				IItemGrid itemGrid = gridState.ItemGrid;
				((GameObject)itemGrid.DisplayContainer).SetActive(itemGrid.IsVisible);
			}
		}

	}

}
