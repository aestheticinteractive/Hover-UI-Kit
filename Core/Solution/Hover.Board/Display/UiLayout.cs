using System;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiLayout : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(LayoutState pLayoutState, IItemVisualSettingsProvider pItemVisualSettProv) {
			Vector2 sumSize = Vector2.zero;

			for ( int i = 0 ; i < pLayoutState.Items.Length ; i++ ) {
				BaseItemState itemState = pLayoutState.Items[i];
				IItemVisualSettings visualSett = pItemVisualSettProv.GetSettings(itemState.Item);
				GameObject itemObj = (GameObject)itemState.Item.DisplayContainer;
				Vector2 pos = Vector2.Scale(sumSize, pLayoutState.ItemLayout.Direction);

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(itemState, visualSett);
				uiItem.transform.localPosition = pos*UiItem.Size;

				sumSize.x += itemState.Item.Width;
				sumSize.y += itemState.Item.Height;
			}

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
