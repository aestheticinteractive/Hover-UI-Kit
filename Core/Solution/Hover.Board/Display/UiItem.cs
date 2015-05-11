using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiItem : MonoBehaviour {

		public const float Size = 1;

		private BaseItemState vItemState;

		private GameObject vRendererObj;
		private IUiItemRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(IHoverboardPanelState pPanelState, IHoverboardLayoutState pLayoutState, 
										BaseItemState pItemState, IItemVisualSettings pVisualSettings) {
			vItemState = pItemState;

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiItemRenderer)vRendererObj.AddComponent(pVisualSettings.Renderer);
			vRenderer.Build(pPanelState, pLayoutState, vItemState, pVisualSettings);

			vItemState.HoverPointUpdater = vRenderer.UpdateHoverPoints;

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
