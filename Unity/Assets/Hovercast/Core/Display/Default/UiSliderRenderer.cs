using System;
using Hovercast.Core.Custom;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiSliderRenderer : MonoBehaviour, IUiSegmentRenderer {

		protected ArcState vArcState;
		protected SegmentState vSegState;
		protected float vAngle0;
		protected float vAngle1;
		protected SegmentSettings vSettings;
		protected NavItemSlider vNavSlider;

		protected float vGrabArcHalf;
		protected float vSlideDegree0;
		protected float vSlideDegrees;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiSlice vHiddenSlice;
		protected UiSlice vTrackA;
		protected UiSlice vTrackB;
		protected UiSlice vFillA;
		protected UiSlice vFillB;

		protected Material vTickMat;
		protected GameObject[] vTicks;

		protected GameObject vGrabHold;
		protected UiSliderGrabRenderer vGrab;

		protected GameObject vHoverHold;
		protected UiSlice vHover;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, SegmentState pSegState,
														float pArcAngle, SegmentSettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vAngle0 = -pArcAngle/2f+UiSlice.AngleInset;
			vAngle1 = pArcAngle/2f-UiSlice.AngleInset;
			vSettings = pSettings;
			vNavSlider = (NavItemSlider)vSegState.NavItem;

			const float pi = (float)Math.PI;
			const float radInner = 1.04f;
			const float radOuter = 1.46f;

			vGrabArcHalf = pi/80f;
			vSlideDegree0 = (vAngle0+vGrabArcHalf)/pi*180;
			vSlideDegrees = (vAngle1-vAngle0-vGrabArcHalf*2)/pi*180;

			////

			vHiddenSlice = new UiSlice(gameObject, true);
			vHiddenSlice.Resize(pArcAngle);
			vHiddenSlice.UpdateBackground(Color.clear);

			vTrackA = new UiSlice(gameObject, true, "TrackA", radInner, radOuter);
			vTrackB = new UiSlice(gameObject, true, "TrackB", radInner, radOuter);
			vFillA = new UiSlice(gameObject, true, "FillA", radInner, radOuter);
			vFillB = new UiSlice(gameObject, true, "FillB", radInner, radOuter);

			////

			vTickMat = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vTickMat.renderQueue -= 400;
			vTickMat.color = Color.clear;

			if ( vNavSlider.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiSlice.AngleInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vNavSlider.Ticks-1);

				vTicks = new GameObject[vNavSlider.Ticks];

				for ( int i = 0 ; i < vNavSlider.Ticks ; ++i ) {
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
			vGrab.Build(vArcState, vSegState, vGrabArcHalf*2, pSettings);

			////

			vHoverHold = new GameObject("HoverHold");
			vHoverHold.transform.SetParent(gameObject.transform, false);

			var hoverObj = new GameObject("Hover");
			hoverObj.transform.SetParent(vHoverHold.transform, false);

			vHover = new UiSlice(hoverObj, false, "Hover");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vArcState)*vAnimAlpha;

			if ( !vSegState.NavItem.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			float easedVal = GetEasedValue(vNavSlider.Value, vNavSlider.SnappedValue);
			float easedHover = (vNavSlider.HoverValue == null ? easedVal : 
				GetEasedValue((float)vNavSlider.HoverValue, (float)vNavSlider.HoverSnappedValue));
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

			if ( vNavSlider.HoverSnappedValue != null ) {
				slideDeg = vSlideDegree0 + vSlideDegrees*easedHover;
				vHoverHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);

				float high = vSegState.HighlightProgress;
				float select = 1-(float)Math.Pow(1-vSegState.SelectionProgress, 1.5f);

				Color colBg = vSettings.BackgroundColor;
				Color colHigh = vSettings.HighlightColor;
				Color colSel = vSettings.SelectionColor;

				colBg.a *= high*vMainAlpha;
				colHigh.a *= high*vMainAlpha;
				colSel.a *= select*vMainAlpha;

				vHover.UpdateBackground(colBg);
				vHover.UpdateHighlight(colHigh, high);
				vHover.UpdateSelect(colSel, select);

				hoverArcHalf = vGrabArcHalf*high*0.25f - UiSlice.AngleInset;
			}

			vHover.Resize(Math.Max(0, hoverArcHalf*2));
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
			return vHiddenSlice.GetPointNearestToCursor(pCursorLocalPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float GetEasedValue(float pValue, float pSnappedValue) {
			if ( vNavSlider.Snaps < 2 ) {
				return pValue;
			}

			float showVal = pSnappedValue;
			int snaps = vNavSlider.Snaps-1;
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
			float grabArc = (vGrabArcHalf+UiSlice.AngleInset)*2;
			float hoverArcPad = vGrabArcHalf-pHoverArcHalf;
			float fullArc = vAngle1-vAngle0-grabArc;
			float valAngle = vAngle0 + fullArc*pValue;
			float hovAngle = vAngle0 + fullArc*pHoverValue;

			if ( pHoverArcHalf == 0 ) {
				vFillA.Resize(vAngle0, valAngle);
				vFillB.Resize(0);
				vTrackA.Resize(grabArc+valAngle, vAngle1);
				vTrackB.Resize(0);
			}
			else if ( pValue <= pHoverValue ) {
				vFillA.Resize(vAngle0, valAngle);
				vFillB.Resize(0);
				vTrackA.Resize(grabArc+valAngle, hovAngle+hoverArcPad);
				vTrackB.Resize(hovAngle+grabArc-hoverArcPad, vAngle1);
			}
			else {
				vFillA.Resize(vAngle0, hovAngle+hoverArcPad);
				vFillB.Resize(hovAngle+grabArc-hoverArcPad, valAngle);
				vTrackA.Resize(grabArc+valAngle, vAngle1);
				vTrackB.Resize(0);
			}
		}

	}

}
