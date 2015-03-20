using System;
using Hover.Board.Custom;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiSelectRenderer : MonoBehaviour, IUiItemRenderer {

		public const float ArcCanvasScale = UiItem.Size*0.012f;

		private IBaseItemState vItemState;
		private ItemVisualSettings vVisualSettings;

		private float vMainAlpha;

		private UiHoverMeshSquare vHoverMesh;
		private UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(IBaseItemState pItemState, IItemVisualSettings pSettings) {
			vItemState = pItemState;
			vVisualSettings = (ItemVisualSettings)pSettings; //uses renderer-specific settings class

			float width = UiItem.Size*vItemState.Item.Width;
			const float height = UiItem.Size;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(width/2, 0, height/2f);

			vHoverMesh = new UiHoverMeshSquare(gameObject);
			vHoverMesh.UpdateSize(width, height);

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

			if ( !vItemState.Item.IsEnabled ) {
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

			vHoverMesh.UpdateBackground(colBg);
			vHoverMesh.UpdateEdge(colEdge);
			vHoverMesh.UpdateHighlight(colHigh, high);
			vHoverMesh.UpdateSelect(colSel, select);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vVisualSettings.TextFont;
			vLabel.FontSize = vVisualSettings.TextSize;
			vLabel.Color = vVisualSettings.TextColor;
			vLabel.Label = vItemState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			return vHoverMesh.GetPointNearestToCursor(pCursorLocalPos);
		}

	}

}
