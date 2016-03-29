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

		protected float vGrabArc;
		protected float vSlideDegree0;
		protected float vSlideDegrees;
		protected float vZeroValue;

		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected UiHoverMeshSlice vHiddenSlice;
		protected UiItemSliderTrackRenderer vTrack;
		protected ReadList<DisplayUtil.TrackSegment> vTrackSegments;
		protected ReadList<DisplayUtil.TrackSegment> vTrackCuts;

		protected GameObject[] vTicks;
		protected MeshBuilder vTickMeshBuilder;

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

			vGrabArc = pi/40f;
			vSlideDegree0 = (vAngle0+vGrabArc/2)/pi*180;
			vSlideDegrees = (vAngle1-vAngle0-vGrabArc)/pi*180;
			vZeroValue = (0-vSliderItem.RangeMin)/(vSliderItem.RangeMax-vSliderItem.RangeMin);

			////

			vHiddenSlice = new UiHoverMeshSlice(gameObject, true);
			vHiddenSlice.UpdateSize(1, 1.5f, pArcAngle);
			vHiddenSlice.UpdateBackground(Color.clear);

			var trackObj = new GameObject("Track");
			trackObj.transform.SetParent(gameObject.transform, false);
			trackObj.transform.localRotation = Quaternion.AngleAxis(vAngle0/pi*180, Vector3.up);

			vTrack = new UiItemSliderTrackRenderer(trackObj);
			vTrackSegments = new ReadList<DisplayUtil.TrackSegment>();
			vTrackCuts = new ReadList<DisplayUtil.TrackSegment>();

			////

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshSlice.AngleInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vSliderItem.Ticks-1);

				vTickMeshBuilder = new MeshBuilder();
				MeshUtil.BuildQuadMesh(vTickMeshBuilder);
				vTickMeshBuilder.Commit();
				vTickMeshBuilder.CommitColors(Color.clear);

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
					quadFilt.sharedMesh = vTickMeshBuilder.Mesh;
				}
			}

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiItemSliderGrabRenderer>();
			vGrab.Build(vMenuState, vItemState, vGrabArc, pSettings);

			////

			vHoverHold = new GameObject("HoverHold");
			vHoverHold.transform.SetParent(gameObject.transform, false);

			var hoverObj = new GameObject("Hover");
			hoverObj.transform.SetParent(vHoverHold.transform, false);

			vHover = new UiHoverMeshSlice(hoverObj, false, "Hover");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vTrack.SetDepthHint(pDepthHint);
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

			if ( !vSliderItem.IsEnabled || !vSliderItem.IsAncestryEnabled ) {
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

			vTrack.SetColors(colTrack, colFill);

			if ( vTickMeshBuilder != null ) {
				vTickMeshBuilder.CommitColors(colTick);
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

				hoverArc = vGrabArc*high*HoverBarRelW;
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
		public virtual void UpdateHoverPoints(IBaseItemPointsState pPointsState, 
																			Vector3 pCursorWorldPos) {
			if ( vSliderItem.AllowJump ) {
				vHiddenSlice.UpdateHoverPoints(pPointsState);
				return;
			}

			vGrab.UpdateHoverPoints(pPointsState, pCursorWorldPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshes(float pValue, float pHoverValue, float pHoverArc) {
			vTrackSegments.Clear();
			vTrackCuts.Clear();

			const float radInn = UiItemSelectRenderer.InnerRadius;
			const float radOut = UiItemSelectRenderer.OuterRadius;
			const float radInset = 0.04f;

			float fullAngle = vAngle1-vAngle0;
			float slideAngle = fullAngle-vGrabArc;
			float grabArcHalf = vGrabArc/2;
			float valCenter = pValue*slideAngle + grabArcHalf;
			float hovCenter = pHoverValue*slideAngle + grabArcHalf;
			float zeroCenter = vZeroValue*slideAngle + grabArcHalf;
			bool isHoverTooClose = (Math.Abs(valCenter-hovCenter) < vGrabArc*(0.5f+HoverBarRelW));

			if ( vSliderItem.FillStartingPoint == SliderItemFillType.Zero ) {
				float break0 = Math.Min(zeroCenter, valCenter);
				float break1 = Math.Max(zeroCenter, valCenter);

				vTrackSegments.Add(new DisplayUtil.TrackSegment {
					StartValue = 0,
					EndValue = break0
				});

				vTrackSegments.Add(new DisplayUtil.TrackSegment {
					StartValue = break0,
					EndValue = break1,
					IsFill = true,
					IsZeroAtStart = (break0 == zeroCenter)
				});

				vTrackSegments.Add(new DisplayUtil.TrackSegment {
					StartValue = break1,
					EndValue = fullAngle
				});
			}
			else {
				bool isMinStart = (vSliderItem.FillStartingPoint == SliderItemFillType.MinimumValue);

				vTrackSegments.Add(new DisplayUtil.TrackSegment {
					StartValue = 0,
					EndValue = valCenter,
					IsFill = isMinStart
				});

				vTrackSegments.Add(new DisplayUtil.TrackSegment {
					StartValue = valCenter,
					EndValue = fullAngle,
					IsFill = !isMinStart
				});
			}

			vTrackCuts.Add(new DisplayUtil.TrackSegment {
				StartValue = valCenter - grabArcHalf,
				EndValue = valCenter + grabArcHalf,
			});

			if ( pHoverArc > 0 && !isHoverTooClose ) {
				vTrackCuts.Add(new DisplayUtil.TrackSegment {
					StartValue = hovCenter - pHoverArc/2,
					EndValue = hovCenter + pHoverArc/2,
				});

				vHover.UpdateSize(radInn, radOut, pHoverArc);
			}
			else {
				vHover.UpdateSize(0, 0, 0);
			}

			vTrack.UpdateSegments(vTrackSegments.ReadOnly, vTrackCuts.ReadOnly,
				radInn+radInset, radOut-radInset);
		}

	}

}
