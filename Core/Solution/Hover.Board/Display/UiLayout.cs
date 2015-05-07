using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiLayout : MonoBehaviour {

		public Rect Bounds { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(LayoutState pLayoutState, IItemVisualSettingsProvider pItemVisualSettProv) {
			Vector2 dir = pLayoutState.ItemLayout.Direction;
			Vector2 pos = Vector2.zero;
			Vector2 posMin = Vector2.zero;
			Vector2 posMax = Vector2.zero;

			for ( int i = 0 ; i < pLayoutState.Items.Length ; i++ ) {
				BaseItemState itemState = pLayoutState.Items[i];
				IItemVisualSettings visualSett = pItemVisualSettProv.GetSettings(itemState.Item);
				GameObject itemObj = (GameObject)itemState.Item.DisplayContainer;

				UiItem uiItem = itemObj.AddComponent<UiItem>();
				uiItem.Build(itemState, visualSett);
				uiItem.transform.localPosition = new Vector3(pos.x, 0, pos.y)*UiItem.Size;

				var itemSize = new Vector2(itemState.Item.Width, itemState.Item.Height);
				
				posMin = Vector2.Min(posMin, pos);
				posMax = Vector2.Max(posMax, pos+itemSize);
				pos += Vector2.Scale(itemSize, dir);
			}

			Vector2 size = posMax-posMin;
			Bounds = new Rect(posMin.x, posMin.y, size.x, size.y);

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
