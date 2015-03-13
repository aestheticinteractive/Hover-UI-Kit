using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiSelectRenderer : MonoBehaviour, IUiSegmentRenderer {

		public const float ArcCanvasThickness = 250;
		public const float ArcCanvasScale = 0.002f;

		protected ArcState vArcState;
		protected SegmentState vSegState;
		protected SegmentSettings vSettings;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiSlice vSlice;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, SegmentState pSegState,
														float pArcAngle, SegmentSettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vSettings = pSettings;

			////

			vSlice = new UiSlice(gameObject);
			vSlice.Resize(pArcAngle);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, 1);
			labelObj.transform.localScale = new Vector3(1, 1, (vArcState.IsLeft ? 1 : -1));
			
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.IsLeft = vArcState.IsLeft;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = GetArcAlpha(vArcState)*vAnimAlpha;

			if ( !vSegState.NavItem.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			float high = vSegState.HighlightProgress;
			float edge = (vSegState.IsNearestHighlight && !vSegState.IsSelectionPrevented && 
				vSegState.NavItem.AllowSelection ? high : 0);
			float select = 1-(float)Math.Pow(1-vSegState.SelectionProgress, 1.5f);
			float selectAlpha = select;

			if ( vSegState.NavItem.IsStickySelected ) {
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

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vSegState.NavItem.Label;
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
		public static float GetArcAlpha(ArcState pArcState) {
			float alpha = 1-(float)Math.Pow(1-pArcState.DisplayStrength, 2);
			alpha -= (float)Math.Pow(pArcState.NavBackStrength, 2);
			return Math.Max(0, alpha);
		}

	}

}
