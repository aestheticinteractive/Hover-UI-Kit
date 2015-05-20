using System;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Items.Types;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderRenderer : MonoBehaviour, IUiItemRenderer {

		public const float HoverBarRelW = 0.333f;

		public static readonly Quaternion TickQuatRot = 
			Quaternion.FromToRotation(Vector3.back, Vector3.down);

		protected IHovercastMenuState vMenuState;
		protected IBaseItemState vItemState;
		protected float vAngle0;
		protected float vAngle1;
		protected ItemVisualSettingsStandard vSettings;
		protected ISliderItem vSliderItem;

		protected float vGrabArcHalf;
		protected float vSlideDegree0;
		protected float vSlideDegrees;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiHoverMeshSlice vHiddenSlice;
		protected UiHoverMeshSlice vTrackA;
		protected UiHoverMeshSlice vTrackB;
		protected UiHoverMeshSlice vFillA;
		protected UiHoverMeshSlice vFillB;

		protected GameObject[] vTicks;
		protected Mesh vTickMesh;

		protected GameObject vGrabHold;
		protected UiItemSliderGrabRenderer vGrab;

		protected GameObject vHoverHold;
		protected UiHoverMeshSlice vHover;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			vAngle0 = -pArcAngle/2f;
			vAngle1 = pArcAngle/2f;
			vSettings = (ItemVisualSettingsStandard)pSettings;
			vSliderItem = (ISliderItem)vItemState.Item;
			vTicks = new GameObject[vSliderItem.Ticks];

			const float pi = (float)Math.PI;

			vGrabArcHalf = pi/80f;
			vSlideDegree0 = (vAngle0+vGrabArcHalf)/pi*180;
			vSlideDegrees = (vAngle1-vAngle0-vGrabArcHalf*2)/pi*180;

			////

			vHiddenSlice = new UiHoverMeshSlice(gameObject, true);
			vHiddenSlice.Resize(1, 1.5f, pArcAngle);
			vHiddenSlice.UpdateBackground(Color.clear);

			vTrackA = new UiHoverMeshSlice(gameObject, true, "TrackA");
			vTrackB = new UiHoverMeshSlice(gameObject, true, "TrackB");
			vFillA = new UiHoverMeshSlice(gameObject, true, "FillA");
			vFillB = new UiHoverMeshSlice(gameObject, true, "FillB");

			////

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshSlice.AngleInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vSliderItem.Ticks-1);

				vTickMesh = new Mesh();
				MeshUtil.BuildQuadMesh(vTickMesh);
				Materials.SetMeshColor(vTickMesh, Color.clear);

				for ( int i = 0 ; i < vSliderItem.Ticks ; ++i ) {
					var tickObj = new GameObject("Tick"+i);
					tickObj.transform.SetParent(gameObject.transform, false);
					tickObj.transform.localRotation = Quaternion.AngleAxis(
						vSlideDegree0+vSlideDegrees*i*percPerTick, Vector3.up);
					vTicks[i] = tickObj;

					var quadObj = new GameObject("Quad");
					quadObj.transform.SetParent(tickObj.transform, false);
					quadObj.transform.localPosition = new Vector3(0, 0, 1.25f);
					quadObj.transform.localRotation = TickQuatRot;
					quadObj.transform.localScale = quadScale;
					quadObj.AddComponent<MeshRenderer>();

					MeshFilter quadFilt = quadObj.AddComponent<MeshFilter>();
					quadFilt.sharedMesh = vTickMesh;
				}
			}

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiItemSliderGrabRenderer>();
			vGrab.Build(vMenuState, vItemState, vGrabArcHalf*2, pSettings);

			////

			vHoverHold = new GameObject("HoverHold");
			vHoverHold.transform.SetParent(gameObject.transform, false);

			var hoverObj = new GameObject("Hover");
			hoverObj.transform.SetParent(vHoverHold.transform, false);

			vHover = new UiHoverMeshSlice(hoverObj, false, "Hover");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vTrackA.SetDepthHint(pDepthHint);
			vTrackB.SetDepthHint(pDepthHint);
			vFillA.SetDepthHint(pDepthHint);
			vFillB.SetDepthHint(pDepthHint);
			vHover.SetDepthHint(pDepthHint);
			vGrab.SetDepthHint(pDepthHint);

			var tickMat = Materials.GetLayer(Materials.Layer.Ticks, pDepthHint);

			foreach ( GameObject tickObj in vTicks ) {
				GameObject quadObj = tickObj.transform.GetChild(0).gameObject;
				quadObj.GetComponent<MeshRenderer>().sharedMaterial = tickMat;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiItemSelectRenderer.GetArcAlpha(vMenuState)*vAnimAlpha;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}

			const int easePower = 3;
			int snaps = vSliderItem.Snaps;
			float easedVal = DisplayUtil.GetEasedValue(
				snaps, vSliderItem.Value, vSliderItem.SnappedValue, easePower);
			float easedHover = easedVal;
			float hoverArc = 0;

			if ( vSliderItem.HoverValue != null ) {
				easedHover = DisplayUtil.GetEasedValue(snaps, (float)vSliderItem.HoverValue,
					(float)vSliderItem.HoverSnappedValue, easePower);
			} 

			Color colTrack = vSettings.SliderTrackColor;
			Color colFill = vSettings.SliderFillColor;
			Color colTick = vSettings.SliderTickColor;

			colTrack.a *= vMainAlpha;
			colFill.a *= vMainAlpha;
			colTick.a *= vMainAlpha;

			vTrackA.UpdateBackground(colTrack);
			vTrackB.UpdateBackground(colTrack);
			vFillA.UpdateBackground(colFill);
			vFillB.UpdateBackground(colFill);

			if ( vTickMesh != null ) {
				Materials.SetMeshColor(vTickMesh, colTick);
			}

			////
			
			float slideDeg = vSlideDegree0 + vSlideDegrees*easedVal;
			vGrabHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);

			if ( vSliderItem.HoverValue != null ) {
				slideDeg = vSlideDegree0 + vSlideDegrees*easedHover;
				vHoverHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);

				float high = vItemState.MaxHighlightProgress;
				float select = 1-(float)Math.Pow(1-vItemState.SelectionProgress, 1.5f);

				Color colBg = vSettings.BackgroundColor;
				Color colHigh = vSettings.HighlightColor;
				Color colSel = vSettings.SelectionColor;

				colBg.a *= high*vMainAlpha;
				colHigh.a *= high*vMainAlpha;
				colSel.a *= select*vMainAlpha;

				vHover.UpdateBackground(colBg);
				vHover.UpdateHighlight(colHigh, high);
				vHover.UpdateSelect(colSel, select);

				hoverArc = vGrabArcHalf*high*HoverBarRelW;
			}

			UpdateMeshes(easedVal, easedHover, hoverArc);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
			vGrab.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			if ( vSliderItem.AllowJump ) {
				vHiddenSlice.UpdateHoverPoints(pPointsState);
				return;
			}

			vGrab.UpdateHoverPoints(pPointsState, pCursorWorldPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes(float pValue, float pHoverValue, float pHoverArc) {
			const float ri1 = 1f;
			const float ro1 = 1.5f;
			const float ri2 = ri1+0.04f;
			const float ro2 = ro1-0.04f;

			float grabArc = (vGrabArcHalf)*2;
			float fullArc = vAngle1-vAngle0-grabArc;
			float valAngle = vAngle0 + fullArc*pValue;
			float hovAngle = vAngle0 + fullArc*pHoverValue;
			float hoverArcPad = vGrabArcHalf-pHoverArc/2;
			bool tooClose = (Math.Abs(valAngle-hovAngle) < grabArc*(0.5f+HoverBarRelW));

			if ( pHoverArc == 0 || tooClose ) {
				vHover.Resize(ri1, ro1, 0);
				vFillA.Resize(ri2, ro2, vAngle0, valAngle);
				vFillB.Resize(ri2, ro2, 0);
				vTrackA.Resize(ri2, ro2, grabArc+valAngle, vAngle1);
				vTrackB.Resize(ri2, ro2, 0);
				return;
			}

			vHover.Resize(ri1, ro1, pHoverArc);

			if ( pValue <= pHoverValue ) {
				vFillA.Resize(ri2, ro2, vAngle0, valAngle);
				vFillB.Resize(ri2, ro2, 0);
				vTrackA.Resize(ri2, ro2, grabArc+valAngle, hovAngle+hoverArcPad);
				vTrackB.Resize(ri2, ro2, hovAngle+grabArc-hoverArcPad, vAngle1);
			}
			else {
				vFillA.Resize(ri2, ro2, vAngle0, hovAngle+hoverArcPad);
				vFillB.Resize(ri2, ro2, hovAngle+grabArc-hoverArcPad, valAngle);
				vTrackA.Resize(ri2, ro2, grabArc+valAngle, vAngle1);
				vTrackB.Resize(ri2, ro2, 0);
			}
		}

	}

}
