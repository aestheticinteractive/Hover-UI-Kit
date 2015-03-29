using System;
using Hover.Board.Custom.Standard;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSelectRenderer : MonoBehaviour, IUiItemRenderer {

		public const float ArcCanvasScale = UiItem.Size*0.012f;

		private IBaseItemState vItemState;
		private ItemVisualSettingsStandard vVisualSettings;

		private float vMainAlpha;

		private UiHoverMeshRect vHoverRect;
		private UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(IBaseItemState pItemState, IItemVisualSettings pSettings) {
			vItemState = pItemState;
			vVisualSettings = (ItemVisualSettingsStandard)pSettings;

			float width = UiItem.Size*vItemState.Item.Width;
			const float height = UiItem.Size;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(width/2, 0, height/2f);

			vHoverRect = new UiHoverMeshRect(gameObject);
			vHoverRect.UpdateSize(width, height);

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.SetSize(width, height, ArcCanvasScale);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vMainAlpha = 1;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.AreParentsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			ISelectableItem selItem = (vItemState.Item as ISelectableItem);
			float high = vItemState.MaxHighlightProgress;
			bool showEdge = (vItemState.IsNearestHighlight && !vItemState.IsSelectionPrevented && 
				selItem != null && selItem.AllowSelection);
			float edge = (showEdge ? high : 0);
			float select = 1-(float)Math.Pow(1-vItemState.SelectionProgress, 1.5f);
			float selectAlpha = select;

			if ( selItem != null && selItem.IsStickySelected ) {
				selectAlpha = 1;
			}

			Color colBg = vVisualSettings.BackgroundColor;
			Color colEdge = vVisualSettings.EdgeColor;
			Color colHigh = vVisualSettings.HighlightColor;
			Color colSel = vVisualSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colEdge.a *= edge*vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= selectAlpha*vMainAlpha;

			vHoverRect.UpdateBackground(colBg);
			vHoverRect.UpdateEdge(colEdge);
			vHoverRect.UpdateHighlight(colHigh, high);
			vHoverRect.UpdateSelect(colSel, select);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vVisualSettings.TextFont;
			vLabel.FontSize = vVisualSettings.TextSize;
			vLabel.Color = vVisualSettings.TextColor;
			vLabel.Label = vItemState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			vHoverRect.UpdateHoverPoints(pPointsState);
		}

	}

}
