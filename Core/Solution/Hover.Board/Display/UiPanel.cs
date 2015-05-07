using Hover.Board.Items;
using Hover.Board.State;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiPanel : MonoBehaviour {

		private LayoutState[] vLayoutStates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiPanel() {
			vLayoutStates = new LayoutState[0];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(PanelState pPanelState, IItemVisualSettingsProvider pItemVisualSettProv) {
			vLayoutStates = pPanelState.Layouts;

			foreach ( LayoutState layoutState in vLayoutStates ) {
				IItemLayout itemLayout = layoutState.ItemLayout;
				GameObject layoutObj = (GameObject)itemLayout.DisplayContainer;
				
				UiLayout uiLayout = layoutObj.AddComponent<UiLayout>();
				uiLayout.Build(layoutState, pItemVisualSettProv);

				Vector2 pos = itemLayout.Position*UiItem.Size - uiLayout.Bounds.position - 
					Vector2.Scale(itemLayout.Anchor, uiLayout.Bounds.size);
				uiLayout.transform.localPosition = new Vector3(pos.x, 0, pos.y);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( LayoutState layoutState in vLayoutStates ) {
				IItemLayout itemLayout = layoutState.ItemLayout;
				((GameObject)itemLayout.DisplayContainer).SetActive(itemLayout.IsVisible);
			}
		}

	}

}
