using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemTextRenderer : MonoBehaviour, IUiItemRenderer {

		public const float LabelCanvasScale = UiItem.Size*0.012f;

		protected IHoverboardPanelState vPanelState;
		protected IHoverboardLayoutState vLayoutState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;

		protected float vMainAlpha;
		protected float vWidth;
		protected float vHeight;

		protected UiHoverMeshRectBg vHoverRect;
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
			vWidth = UiItem.Size*vItemState.Item.Width;
			vHeight = UiItem.Size*vItemState.Item.Height;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(vWidth/2, 0, vHeight/2f);

			vHoverRect = new UiHoverMeshRectBg(gameObject);
			vHoverRect.UpdateSize(vWidth, vHeight);

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.SetSize(vWidth, vHeight, vSettings.TextSize*0.25f, LabelCanvasScale);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vHoverRect.SetDepthHint(pDepthHint);
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
			vMainAlpha = vPanelState.DisplayStrength*vLayoutState.DisplayStrength;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			vHoverRect.UpdateBackground(colBg);

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vItemState.Item.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			//do nothing...
		}

	}

}
