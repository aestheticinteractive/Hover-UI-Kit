using System;
using Hover.Board.Custom;
using Hover.Board.State;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiGrid : MonoBehaviour {

		private GridState vGridState;
		private ICustomSegment vCustom;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(GridState pGrid, ICustomSegment pCustom) {
			vGridState = pGrid;
			vCustom = pCustom;

			int cols = vGridState.ItemGrid.Cols;
			int gi = 0;

			for ( int i = 0 ; i < vGridState.Items.Length ; i++ ) {
				BaseItemState button = vGridState.Items[i];
				int w = (int)button.Item.Width;
				var pos = new Vector3(gi%cols, 0, (float)Math.Floor((float)gi/cols));
				GameObject itemObj = (GameObject)button.Item.DisplayContainer;

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(button, vCustom);
				uiItem.transform.localPosition = pos*UiItem.Size;

				gi += w;
			}

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
