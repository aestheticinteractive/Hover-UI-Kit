using System;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiSelectRenderer : MonoBehaviour, IUiItemRenderer {

		public const float ArcCanvasThickness = 250;
		public const float ArcCanvasScale = 0.002f;

		protected MenuState vMenuState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiHoverMeshSlice vSlice;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(MenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			vSettings = (ItemVisualSettingsStandard)pSettings;

			////

			vSlice = new UiHoverMeshSlice(gameObject);
			vSlice.Resize(1, 1.5f, pArcAngle);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, 1);
			labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			labelObj.transform.localScale = new Vector3(1, 1, (vMenuState.IsOnLeftSide ? 1 : -1));
			
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.AlignLeft = vMenuState.IsOnLeftSide;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = GetArcAlpha(vMenuState)*vAnimAlpha;

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

			Color colBg = vSettings.BackgroundColor;
			Color colEdge = vSettings.EdgeColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colEdge.a *= edge*vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= selectAlpha*vMainAlpha;

			vSlice.UpdateBackground(colBg);
			vSlice.UpdateEdge(colEdge);
			vSlice.UpdateHighlight(colHigh, high);
			vSlice.UpdateSelect(colSel, select);

			if ( vSettings.TextSize != vLabel.FontSize ) {
				vLabel.SetSize(ArcCanvasThickness*ArcCanvasScale, 
					vSettings.TextSize*1.5f*ArcCanvasScale, ArcCanvasScale);
			}

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vItemState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			return vSlice.GetPointNearestToCursor(pCursorLocalPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetArcAlpha(MenuState pMenuState) {
			float alpha = 1-(float)Math.Pow(1-pMenuState.DisplayStrength, 2);
			alpha -= (float)Math.Pow(pMenuState.NavBackStrength, 2);
			return Math.Max(0, alpha);
		}

	}

}
