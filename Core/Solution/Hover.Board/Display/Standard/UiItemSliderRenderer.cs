using System;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Items.Types;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderRenderer : MonoBehaviour, IUiItemRenderer {

		public const float HoverBarRelW = 0.333f;

		protected IHoverboardPanelState vPanelState;
		protected IHoverboardLayoutState vLayoutState;
		protected IBaseItemState vItemState;
		protected ItemVisualSettingsStandard vSettings;
		protected ISliderItem vSliderItem;

		protected float vMainAlpha;
		protected float vWidth;
		protected float vHeight;
		protected float vGrabW;

		protected UiHoverMeshRect vHiddenRect;
		protected UiHoverMeshRectBg vTrackA;
		protected UiHoverMeshRectBg vTrackB;
		protected UiHoverMeshRectBg vFillA;
		protected UiHoverMeshRectBg vFillB;

		protected Material vTickMat;
		protected GameObject[] vTicks;

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

			vWidth = UiItem.Size*vItemState.Item.Width;
			vHeight = UiItem.Size*vItemState.Item.Height;
			vGrabW = 1;

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(vWidth/2, 0, vHeight/2f);

			////

			vHiddenRect = new UiHoverMeshRect(gameObject);
			vHiddenRect.UpdateSize(vWidth, vHeight);
			vHiddenRect.UpdateBackground(Color.clear);

			vTrackA = new UiHoverMeshRectBg(gameObject, "TrackA");
			vTrackB = new UiHoverMeshRectBg(gameObject, "TrackB");
			vFillA = new UiHoverMeshRectBg(gameObject, "FillA");
			vFillB = new UiHoverMeshRectBg(gameObject, "FillB");

			////

			/*vTickMat = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vTickMat.renderQueue -= 400;
			vTickMat.color = Color.clear;

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshRect.AngleInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vSliderItem.Ticks-1);

				vTicks = new GameObject[vSliderItem.Ticks];

				for ( int i = 0 ; i < vSliderItem.Ticks ; ++i ) {
					var tick = new GameObject("Tick"+i);
					tick.transform.SetParent(gameObject.transform, false);
					tick.transform.localRotation = Quaternion.AngleAxis(
						vSlideDegree0+vSlideDegrees*i*percPerTick, Vector3.up);
					vTicks[i] = tick;

					var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.renderer.sharedMaterial = vTickMat;
					quad.transform.SetParent(tick.transform, false);
					quad.transform.localPosition = new Vector3(0, 0, 1.25f);
					quad.transform.localRotation = 
						Quaternion.FromToRotation(Vector3.back, Vector3.down);
					quad.transform.localScale = quadScale;
				}
			}*/

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
			//vTickMat.color = colTick;

			////

			Vector3 slideOrigin = new Vector3((vGrabW-vWidth)/2, 0, 0);

			vGrabHold.transform.localPosition = 
				slideOrigin + new Vector3((vWidth-vGrabW)*easedVal, 0, 0);

			if ( vSliderItem.HoverValue != null ) {
				vHoverHold.transform.localPosition = 
					slideOrigin + new Vector3((vWidth-vGrabW)*easedHover, 0, 0);

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
			float fullW = vWidth-vGrabW;
			float valPos = fullW*pValue;
			float hovPos = fullW*pHoverValue;
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
