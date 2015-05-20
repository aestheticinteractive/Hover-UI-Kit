using System;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Items.Types;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderRenderer : MonoBehaviour, IUiItemRenderer {

		public const float HoverBarRelW = 0.333f;

		public static readonly Quaternion TickQuatRot = 
			Quaternion.FromToRotation(Vector3.back, Vector3.down);

		protected IHoverboardPanelState vPanelState;
		protected IHoverboardLayoutState vLayoutState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;
		protected ISliderItem vSliderItem;

		protected float vMainAlpha;
		protected float vWidth;
		protected float vHeight;
		protected float vGrabW;
		protected float vSlideX0;
		protected float vSlideW;

		protected UiHoverMeshRectBg vHiddenRect;
		protected UiHoverMeshRectBg vTrackA;
		protected UiHoverMeshRectBg vTrackB;
		protected UiHoverMeshRectBg vFillA;
		protected UiHoverMeshRectBg vFillB;

		protected GameObject[] vTicks;
		protected Mesh vTickMesh;

		protected GameObject vGrabHold;
		protected UiItemSliderGrabRenderer vGrab;

		protected GameObject vHoverHold;
		protected UiHoverMeshRect vHover;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(IHoverboardPanelState pPanelState,
										IHoverboardLayoutState pLayoutState, IBaseItemState pItemState,
										IItemVisualSettings pSettings) {
			vPanelState = pPanelState;
			vLayoutState = pLayoutState;
			vItemState = pItemState;
			vSettings = (ItemVisualSettingsStandard)pSettings;
			vSliderItem = (ISliderItem)vItemState.Item;
			vTicks = new GameObject[vSliderItem.Ticks];

			vWidth = UiItem.Size*vItemState.Item.Width;
			vHeight = UiItem.Size*vItemState.Item.Height;
			vGrabW = 1;
			vSlideX0 = (vGrabW-vWidth)/2;
			vSlideW = vWidth-vGrabW;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(vWidth/2, 0, vHeight/2f);

			////

			vHiddenRect = new UiHoverMeshRectBg(gameObject);
			vHiddenRect.UpdateSize(vWidth, vHeight);

			vTrackA = new UiHoverMeshRectBg(gameObject, "TrackA");
			vTrackB = new UiHoverMeshRectBg(gameObject, "TrackB");
			vFillA = new UiHoverMeshRectBg(gameObject, "FillA");
			vFillB = new UiHoverMeshRectBg(gameObject, "FillB");

			////

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshRect.SizeInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vSliderItem.Ticks-1);

				vTickMesh = new Mesh();
				MeshUtil.BuildQuadMesh(vTickMesh);
				Materials.SetMeshColor(vTickMesh, Color.clear);

				for ( int i = 0 ; i < vSliderItem.Ticks ; ++i ) {
					GameObject tickObj = new GameObject("Tick"+i);
					tickObj.transform.SetParent(gameObject.transform, false);
					tickObj.transform.localPosition = Vector3.right*(vSlideX0+vSlideW*i*percPerTick);
					tickObj.transform.localRotation = TickQuatRot;
					tickObj.transform.localScale = quadScale;
					tickObj.AddComponent<MeshRenderer>();

					MeshFilter tickFilt = tickObj.AddComponent<MeshFilter>();
					tickFilt.sharedMesh = vTickMesh;

					vTicks[i] = tickObj;
				}
			}

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiItemSliderGrabRenderer>();
			vGrab.Build(vPanelState, vLayoutState, vItemState, vSettings);
			vGrab.SetCustomSize(vGrabW, vHeight, false);

			////

			vHoverHold = new GameObject("HoverHold");
			vHoverHold.transform.SetParent(gameObject.transform, false);

			var hoverObj = new GameObject("Hover");
			hoverObj.transform.SetParent(vHoverHold.transform, false);

			vHover = new UiHoverMeshRect(hoverObj, "Hover");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vHiddenRect.SetDepthHint(pDepthHint);
			vTrackA.SetDepthHint(pDepthHint);
			vTrackB.SetDepthHint(pDepthHint);
			vFillA.SetDepthHint(pDepthHint);
			vFillB.SetDepthHint(pDepthHint);
			vHover.SetDepthHint(pDepthHint);
			vGrab.SetDepthHint(pDepthHint);

			var tickMat = Materials.GetLayer(Materials.Layer.Ticks, pDepthHint);

			foreach ( GameObject tickObj in vTicks ) {
				tickObj.GetComponent<MeshRenderer>().sharedMaterial = tickMat;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = vPanelState.DisplayStrength*vLayoutState.DisplayStrength;

			if ( !vItemState.Item.IsEnabled || !vItemState.Item.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}

			const int easePower = 3;
			int snaps = vSliderItem.Snaps;
			float easedVal = EasingUtil.GetEasedValue(
				snaps, vSliderItem.Value, vSliderItem.SnappedValue, easePower);
			float easedHover = easedVal;
			float hoverW = 0;

			if ( vSliderItem.HoverValue != null ) {
				easedHover = EasingUtil.GetEasedValue(snaps, (float)vSliderItem.HoverValue, 
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

			vGrabHold.transform.localPosition = new Vector3(vSlideX0+vSlideW*easedVal, 0, 0);

			if ( vSliderItem.HoverValue != null ) {
				vHoverHold.transform.localPosition = new Vector3(vSlideX0+vSlideW*easedHover, 0, 0);

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

				hoverW = vGrabW*high*HoverBarRelW;
			}

			UpdateMeshes(easedVal, easedHover, hoverW);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			if ( vSliderItem.AllowJump ) {
				vHiddenRect.UpdateHoverPoints(pPointsState);
				return;
			}

			vGrab.UpdateHoverPoints(pPointsState, pCursorWorldPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes(float pValue, float pHoverValue, float pHoverW) {
			float valPos = vSlideW*pValue;
			float hovPos = vSlideW*pHoverValue;
			float hoverWPad = (vGrabW-pHoverW)/2;
			bool tooClose = (Math.Abs(valPos-hovPos) < vGrabW*(0.5f+HoverBarRelW));

			float trackH = vHeight*0.92f;
			Vector3 origin = new Vector3(-vWidth/2, 0, 0);
			Vector3 xPos = Vector3.right;
			float x0, x1;

			Action<UiHoverMeshRectBg, float, float> setPos = ((bg, startX, endX) => {
				bg.Show(endX != startX);
				bg.UpdateSize(endX-startX, trackH);
				bg.Background.transform.localPosition = origin + xPos*startX;
			});

			if ( pHoverW == 0 || tooClose ) {
				vHover.UpdateSize(0, 0);

				x0 = 0;
				x1 = valPos;
				setPos(vFillA, x0, x1);

				vFillB.Show(false);

				x0 = x1+vGrabW;
				x1 = vWidth;
				setPos(vTrackA, x0, x1);

				vTrackB.Show(false);
				return;
			}

			vHover.UpdateSize(pHoverW, vHeight);

			if ( pValue <= pHoverValue ) {
				x0 = 0;
				x1 = valPos;
				setPos(vFillA, x0, x1);

				vFillB.Show(false);

				x0 = valPos+vGrabW;
				x1 = hovPos+hoverWPad;
				setPos(vTrackA, x0, x1);

				x0 = hovPos+vGrabW-hoverWPad;
				x1 = vWidth;
				setPos(vTrackB, x0, x1);
			}
			else {
				x0 = 0;
				x1 = hovPos+hoverWPad;
				setPos(vFillA, x0, x1);

				x0 = hovPos+vGrabW-hoverWPad;
				x1 = valPos;
				setPos(vFillB, x0, x1);

				x0 = valPos+vGrabW;
				x1 = vWidth;
				setPos(vTrackA, x0, x1);

				vTrackB.Show(false);
			}
		}

	}

}
