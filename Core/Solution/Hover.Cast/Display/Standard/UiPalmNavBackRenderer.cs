using System;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiPalmNavBackRenderer : MonoBehaviour, IUiItemRenderer {

		public const float InnerRadius = 0;
		public const float OuterRadius = UiPalmRenderer.InnerRadius-UiHoverMeshSlice.EdgeThick-0.005f;

		private static readonly Texture2D IconTex = Resources.Load<Texture2D>("Parent");

		protected MenuState vMenuState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;

		protected UiHoverMeshSlice vHoverSlice;
		protected GameObject vIcon;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(MenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			vSettings = (ItemVisualSettingsStandard)pSettings;

			////

			vHoverSlice = new UiHoverMeshSlice(gameObject);
			vHoverSlice.DrawOuterEdge = true;
			vHoverSlice.Resize(InnerRadius, OuterRadius, pArcAngle);

			////

			vIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vIcon.name = "Icon";
			vIcon.transform.SetParent(gameObject.transform, false);
			vIcon.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up)*
				Quaternion.FromToRotation(Vector3.right, Vector3.up);
			vIcon.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vIcon.renderer.sharedMaterial.color = Color.clear;
			vIcon.renderer.sharedMaterial.mainTexture = IconTex;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			ISelectableItem selItem = (vItemState.Item as ISelectableItem);

			float high = vItemState.MaxHighlightProgress;
			bool showEdge = (vItemState.IsNearestHighlight && !vItemState.IsSelectionPrevented && 
				selItem != null && selItem.AllowSelection);
			float edge = (showEdge ? high : 0);
			float select = 1-(float)Math.Pow(1-vItemState.SelectionProgress, 1.5f);
			float alpha = Math.Max(0, 1-(float)Math.Pow(1-vMenuState.DisplayStrength, 2));

			if ( vMenuState.NavBackStrength > select ) {
				select = vMenuState.NavBackStrength;
			}

			Color colBg = vSettings.BackgroundColor;
			Color colEdge = vSettings.EdgeColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;
			Color colIcon = vSettings.ArrowIconColor;

			colBg.a *= alpha*(vItemState.Item.IsEnabled ? 1 : 0.333f);
			colEdge.a *= edge*alpha;
			colHigh.a *= high*alpha;
			colSel.a *= select*alpha;
			colIcon.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*alpha*
				(vItemState.Item.IsEnabled ? 1 : 0);

			vHoverSlice.UpdateBackground(colBg);
			vHoverSlice.UpdateEdge(colEdge);
			vHoverSlice.UpdateHighlight(colHigh, high);
			vHoverSlice.UpdateSelect(colSel, select);

			vIcon.renderer.sharedMaterial.color = colIcon;
			vIcon.transform.localScale = Vector3.one*vSettings.TextSize*0.75f*
				UiItemSelectRenderer.ArcCanvasScale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			vHoverSlice.UpdateHoverPoints(pPointsState);
		}

	}

}
