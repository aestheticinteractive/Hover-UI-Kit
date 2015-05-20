using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Items.Types;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiItem : MonoBehaviour {

		public const float Size = 1;

		private IHoverboardPanelState vPanelState;
		private BaseItemState vItemState;

		private float vSlideX0;
		private float vSlideW;

		private GameObject vRendererObj;
		private IUiItemRenderer vRenderer;
		private int vPrevDepth;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(IHoverboardPanelState pPanelState, IHoverboardLayoutState pLayoutState, 
										BaseItemState pItemState, IItemVisualSettings pVisualSettings) {
			vPanelState = pPanelState;
			vItemState = pItemState;

			vSlideX0 = 0.5f;
			vSlideW = vItemState.Item.Width-vSlideX0*2;

			////
			
			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiItemRenderer)vRendererObj.AddComponent(pVisualSettings.Renderer);
			vRenderer.Build(pPanelState, pLayoutState, vItemState, pVisualSettings);
			vRenderer.SetDepthHint(vPanelState.DisplayDepthHint);
			vPrevDepth = vPanelState.DisplayDepthHint;

			vItemState.HoverPointUpdater = vRenderer.UpdateHoverPoints;

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			if ( vPanelState.DisplayDepthHint != vPrevDepth ) {
				vRenderer.SetDepthHint(vPanelState.DisplayDepthHint);
				vPrevDepth = vPanelState.DisplayDepthHint;
			}

			TryUpdateSliderValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateSliderValue() {
			ISliderItem sliderItem = (vItemState.Item as ISliderItem);

			if ( sliderItem == null ) {
				return;
			}

			Vector3? cursorWorld = vItemState.NearestCursorWorldPos;
			bool ignoreSlider = (!vItemState.IsNearestHighlight || 
				vItemState.MaxHighlightProgress <= 0 || cursorWorld == null);

			if ( ignoreSlider ) {
				sliderItem.HoverValue = null;
				return;
			}

			Vector3 cursorLocal = gameObject.transform.InverseTransformPoint((Vector3)cursorWorld);
			float value = (cursorLocal.x-vSlideX0)/vSlideW;

			if ( sliderItem.IsStickySelected ) {
				sliderItem.Value = value;
				sliderItem.HoverValue = null;
			}
			else if ( sliderItem.AllowJump ) {
				sliderItem.HoverValue = value;
			}
			else {
				sliderItem.HoverValue = null;
			}
		}

	}

}
