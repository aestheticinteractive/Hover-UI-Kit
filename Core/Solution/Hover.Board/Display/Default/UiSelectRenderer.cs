using System;
using Hover.Board.Custom;
using Hover.Board.State;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Display.Default {

	/*================================================================================================*/
	public class UiSelectRenderer : MonoBehaviour, IUiSegmentRenderer {

		public const float ArcCanvasScale = UiButton.Size*0.012f;

		private ButtonState vButtonState;
		private ButtonSettings vSettings;

		private float vMainAlpha;

		private UiFill vFill;
		private UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ButtonState pButtonState, ButtonSettings pSettings) {
			vButtonState = pButtonState;
			vSettings = pSettings;

			float width = UiButton.Size*vButtonState.Item.Width;
			const float height = UiButton.Size;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(width/2, 0, height/2f);

			vFill = new UiFill(gameObject);
			vFill.UpdateSize(width, height);

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.SetSize(width, height);
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

			if ( !vButtonState.Item.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			ISelectableItem selItem = (vButtonState.Item as ISelectableItem);
			float high = vButtonState.MaxHighlightProgress;
			bool showEdge = (vButtonState.IsNearestHighlight && !vButtonState.IsSelectionPrevented && 
				selItem != null && selItem.AllowSelection);
			float edge = (showEdge ? high : 0);
			float select = 1-(float)Math.Pow(1-vButtonState.SelectionProgress, 1.5f);
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

			vFill.UpdateBackground(colBg);
			vFill.UpdateEdge(colEdge);
			vFill.UpdateHighlight(colHigh, high);
			vFill.UpdateSelect(colSel, select);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vButtonState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			return vFill.GetPointNearestToCursor(pCursorLocalPos);
		}

	}

}
