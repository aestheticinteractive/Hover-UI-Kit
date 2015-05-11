using System;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSelectRenderer : MonoBehaviour, IUiItemRenderer {

		public const float LabelCanvasScale = UiItem.Size*0.012f;

		protected IHoverboardPanelState vPanelState;
		protected IHoverboardLayoutState vLayoutState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;

		protected float vMainAlpha;

		protected UiHoverMeshRect vHoverRect;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(IHoverboardPanelState pPanelState, 
										IHoverboardLayoutState pLayoutState, IBaseItemState pItemState,
										IItemVisualSettings pSettings) {
			vPanelState = pPanelState;
			vLayoutState = pLayoutState;
			vItemState = pItemState;
			vSettings = (ItemVisualSettingsStandard)pSettings;

			float width = UiItem.Size*vItemState.Item.Width;
			float height = UiItem.Size*vItemState.Item.Height;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(width/2, 0, height/2f);

			vHoverRect = new UiHoverMeshRect(gameObject);
			vHoverRect.UpdateSize(width, height);

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.SetSize(width, height, vSettings.TextSize*0.25f, LabelCanvasScale);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = vPanelState.DisplayStrength*vLayoutState.DisplayStrength;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.IsAncestryEnabled ) {
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

			Color colBg = vSettings.BackgroundColor;
			Color colEdge = vSettings.EdgeColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colEdge.a *= edge*vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= selectAlpha*vMainAlpha;

			vHoverRect.UpdateBackground(colBg);
			vHoverRect.UpdateEdge(colEdge);
			vHoverRect.UpdateHighlight(colHigh, high);
			vHoverRect.UpdateSelect(colSel, select);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vItemState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			vHoverRect.UpdateHoverPoints(pPointsState);
		}

	}

}
