using System;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Items.Types;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiSliderRenderer : MonoBehaviour, IUiItemRenderer {

		protected MenuState vMenuState;
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

		protected Material vTickMat;
		protected GameObject[] vTicks;

		protected GameObject vGrabHold;
		protected UiSliderGrabRenderer vGrab;

		protected GameObject vHoverHold;
		protected UiHoverMeshSlice vHover;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(MenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			vAngle0 = -pArcAngle/2f+UiHoverMeshSlice.AngleInset;
			vAngle1 = pArcAngle/2f-UiHoverMeshSlice.AngleInset;
			vSettings = (ItemVisualSettingsStandard)pSettings;
			vSliderItem = (ISliderItem)vItemState.Item;

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

			vTickMat = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vTickMat.renderQueue -= 400;
			vTickMat.color = Color.clear;

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshSlice.AngleInset*2, 0.36f, 0.1f);
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
			}

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiSliderGrabRenderer>();
			vGrab.Build(vMenuState, vItemState, vGrabArcHalf*2, pSettings);

			////

			vHoverHold = new GameObject("HoverHold");
			vHoverHold.transform.SetParent(gameObject.transform, false);

			var hoverObj = new GameObject("Hover");
			hoverObj.transform.SetParent(vHoverHold.transform, false);

			vHover = new UiHoverMeshSlice(hoverObj, false, "Hover");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vMenuState)*vAnimAlpha;

			if ( !vItemState.Item.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			float easedVal = GetEasedValue(vSliderItem.Value, vSliderItem.SnappedValue);
			float easedHover = (vSliderItem.HoverValue == null ? easedVal : 
				GetEasedValue((float)vSliderItem.HoverValue, (float)vSliderItem.HoverSnappedValue));
			float hoverArcHalf = 0;

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
			vTickMat.color = colTick;

			float slideDeg = vSlideDegree0 + vSlideDegrees*easedVal;
			vGrabHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);

			if ( vSliderItem.HoverSnappedValue != null ) {
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

				hoverArcHalf = vGrabArcHalf*high*0.333f - UiHoverMeshSlice.AngleInset;
			}

			UpdateMeshes(easedVal, easedHover, hoverArcHalf);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
			vGrab.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			if ( vSliderItem.AllowJump ) {
				return vHiddenSlice.GetPointNearestToCursor(pCursorLocalPos);
			}

			Transform objTx = gameObject.transform;
			Transform grabTx = vGrab.gameObject.transform;

			Vector3 cursorWorld = objTx.TransformPoint(pCursorLocalPos);
			Vector3 cursorGrab = grabTx.InverseTransformPoint(cursorWorld);

			Vector3 nearestGrab = vGrab.GetPointNearestToCursor(cursorGrab);
			Vector3 nearestWorld = grabTx.TransformPoint(nearestGrab);

			return objTx.InverseTransformPoint(nearestWorld);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float GetEasedValue(float pValue, float pSnappedValue) {
			if ( vSliderItem.Snaps < 2 ) {
				return pValue;
			}

			float showVal = pSnappedValue;
			int snaps = vSliderItem.Snaps-1;
			float diff = pValue-showVal;
			int sign = Math.Sign(diff);

			diff = Math.Abs(diff); //between 0 and 1
			diff *= snaps;

			if ( diff < 0.5 ) {
				diff *= 2;
				diff = (float)Math.Pow(diff, 3);
				diff /= 2f;
			}
			else {
				diff = (diff-0.5f)*2;
				diff = 1-(float)Math.Pow(1-diff, 3);
				diff = diff/2f+0.5f;
			}

			diff /= snaps;
			return showVal + diff*sign;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes(float pValue, float pHoverValue, float pHoverArcHalf) {
			const float ri1 = 1f;
			const float ro1 = 1.5f;
			const float ri2 = ri1+0.04f;
			const float ro2 = ro1-0.04f;

			float grabArc = (vGrabArcHalf+UiHoverMeshSlice.AngleInset)*2;
			float hoverArc = Math.Max(0, pHoverArcHalf*2);
			float hoverArcPad = vGrabArcHalf-pHoverArcHalf;
			float fullArc = vAngle1-vAngle0-grabArc;
			float valAngle = vAngle0 + fullArc*pValue;
			float hovAngle = vAngle0 + fullArc*pHoverValue;
			bool tooClose = (Math.Abs(valAngle-hovAngle) < grabArc/2+hoverArc);

			if ( pHoverArcHalf == 0 || tooClose ) {
				vHover.Resize(ri1, ro1, 0);
				vFillA.Resize(ri2, ro2, vAngle0, valAngle);
				vFillB.Resize(ri2, ro2, 0);
				vTrackA.Resize(ri2, ro2, grabArc+valAngle, vAngle1);
				vTrackB.Resize(ri2, ro2, 0);
				return;
			}

			vHover.Resize(ri1, ro1, hoverArc);

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
