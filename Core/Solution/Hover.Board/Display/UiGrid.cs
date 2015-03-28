using System;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiGrid : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(GridState pGridState, IItemVisualSettingsProvider pItemVisualSettProv) {
			int cols = pGridState.ItemGrid.Cols;
			int gi = 0;

			for ( int i = 0 ; i < pGridState.Items.Length ; i++ ) {
				BaseItemState itemState = pGridState.Items[i];
				IItemVisualSettings visualSett = pItemVisualSettProv.GetSettings(itemState.Item);
				GameObject itemObj = (GameObject)itemState.Item.DisplayContainer;
				var pos = new Vector3(gi%cols, 0, (float)Math.Floor((float)gi/cols));

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(itemState, visualSett);
				uiItem.transform.localPosition = pos*UiItem.Size;

				gi += (int)itemState.Item.Width;
			}

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
