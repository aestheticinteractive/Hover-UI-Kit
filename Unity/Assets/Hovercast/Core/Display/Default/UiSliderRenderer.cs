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
		protected int vMeshSteps;

		protected float vSliderAngleHalf;
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
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			const float pi = (float)Math.PI;
			const float radInner = 1.04f;
			const float radOuter = 1.46f;

			vSliderAngleHalf = pi/80f;
			vSlideDegree0 = (vAngle0+vSliderAngleHalf)/pi*180;
			vSlideDegrees = (vAngle1-vAngle0-vSliderAngleHalf*2)/pi*180;

			////

			vHiddenSlice = new UiSlice(gameObject, true);
			vHiddenSlice.Resize(pArcAngle);
			vHiddenSlice.UpdateBackground(Color.clear);

			vTrackA = new UiSlice(gameObject, true, "TrackA", radInner, radOuter);
			vTrackA.Resize(pArcAngle);

			vTrackB = new UiSlice(gameObject, true, "TrackB", radInner, radOuter);
			vTrackB.Resize(pArcAngle);

			vFillA = new UiSlice(gameObject, true, "FillA", radInner, radOuter);
			vFillA.Resize(pArcAngle);

			vFillB = new UiSlice(gameObject, true, "FillB", radInner, radOuter);
			vFillB.Resize(pArcAngle);

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
			vGrab.Build(vArcState, vSegState, vSliderAngleHalf*2, pSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vArcState)*vAnimAlpha;

			if ( !vSegState.NavItem.IsEnabled ) {
				vMainAlpha *= 0.333f;
			}

			float showVal = GetEasedValue();

			BuildMesh(vTrackA, showVal, 1, false);
			BuildMesh(vTrackB, showVal, showVal, false);
			BuildMesh(vFillA, 0, showVal, true);
			BuildMesh(vFillB, showVal, showVal, true);

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

			float slideDeg = vSlideDegree0 + vSlideDegrees*showVal;
			vGrabHold.transform.localRotation = Quaternion.AngleAxis(slideDeg, Vector3.up);
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
		private float GetEasedValue() {
			float currVal = vNavSlider.Value;

			if ( vNavSlider.Snaps < 2 ) {
				return currVal;
			}

			float showVal = vNavSlider.SnappedValue;
			int snaps = vNavSlider.Snaps-1;
			float diff = currVal-showVal;
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
		private void BuildMesh(UiSlice pSlice, float pAmount0, float pAmount1, bool pIsFill) {
			float sliderAngle = (vSliderAngleHalf+UiSlice.AngleInset)*2;
			float angleRange = vAngle1-vAngle0-sliderAngle;
			float a0 = vAngle0 + angleRange*pAmount0;
			float a1 = vAngle0 + angleRange*pAmount1;

			if ( !pIsFill ) {
				a0 += sliderAngle;
				a1 += sliderAngle;
			}

			pSlice.Resize(a0, a1);
		}

	}

}
