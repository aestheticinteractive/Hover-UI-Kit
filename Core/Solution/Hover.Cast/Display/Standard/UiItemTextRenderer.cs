using System;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemTextRenderer : MonoBehaviour, IUiItemRenderer {

		protected IHovercastMenuState vMenuState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiHoverMeshSlice vHoverSlice;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			vSettings = (ItemVisualSettingsStandard)pSettings;

			////

			vHoverSlice = new UiHoverMeshSlice(gameObject, true);
			vHoverSlice.Resize(UiItemSelectRenderer.InnerRadius, 
				UiItemSelectRenderer.OuterRadius, pArcAngle);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, 1);
			labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			labelObj.transform.localScale = new Vector3((vMenuState.IsOnLeftSide ? 1 : -1), 1, 1);
			
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.AlignLeft = vMenuState.IsOnLeftSide;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vHoverSlice.SetDepthHint(pDepthHint);
			vLabel.SetDepthHint(pDepthHint);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiItemSelectRenderer.GetArcAlpha(vMenuState)*vAnimAlpha;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			vHoverSlice.UpdateBackground(colBg);

			if ( vSettings.TextSize != vLabel.FontSize ) {
				vLabel.SetSize(
					UiItemSelectRenderer.ArcCanvasThickness*UiItemSelectRenderer.ArcCanvasScale,
					vSettings.TextSize*1.5f*UiItemSelectRenderer.ArcCanvasScale,
					vSettings.TextSize*0.6f,
					UiItemSelectRenderer.ArcCanvasScale
				);
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
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			//do nothing...
		}

	}

}
