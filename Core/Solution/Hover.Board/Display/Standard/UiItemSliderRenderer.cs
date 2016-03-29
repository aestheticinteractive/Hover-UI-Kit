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
		protected bool vIsVert;
		protected float vGrabW;
		protected float vSlideX0;
		protected float vSlideW;
		protected float vZeroValue;

		protected UiHoverMeshRectBg vHiddenRect;
		protected UiItemSliderTrackRenderer vTrack;
		protected ReadList<DisplayUtil.TrackSegment> vTrackSegments;
		protected ReadList<DisplayUtil.TrackSegment> vTrackCuts;

		protected GameObject[] vTicks;
		protected MeshBuilder vTickMeshBuilder;

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

			vWidth = UiItem.Size*vSliderItem.Width;
			vHeight = UiItem.Size*vSliderItem.Height;
			vIsVert = (vHeight > vWidth);
			vGrabW = 1;
			vZeroValue = (0-vSliderItem.RangeMin)/(vSliderItem.RangeMax-vSliderItem.RangeMin);

			gameObject.transform.SetParent(gameObject.transform, false);
			gameObject.transform.localPosition = new Vector3(vWidth/2, 0, vHeight/2f);
			gameObject.transform.localRotation = Quaternion.AngleAxis((vIsVert ? 90 : 0), Vector3.up);

			if ( vIsVert ) { //swap dimensions here + rotate graphics later
				float tempW = vWidth;
				vWidth = vHeight;
				vHeight = tempW;
			}

			vSlideX0 = (vGrabW-vWidth)/2;
			vSlideW = vWidth-vGrabW;

			////

			vHiddenRect = new UiHoverMeshRectBg(gameObject);
			vHiddenRect.UpdateSize(vWidth, vHeight);

			var trackObj = new GameObject("Track");
			trackObj.transform.SetParent(gameObject.transform, false);
			trackObj.transform.localPosition = new Vector3(-vWidth/2, 0, 0);

			vTrack = new UiItemSliderTrackRenderer(trackObj);
			vTrackSegments = new ReadList<DisplayUtil.TrackSegment>();
			vTrackCuts = new ReadList<DisplayUtil.TrackSegment>();

			////

			if ( vSliderItem.Ticks > 1 ) {
				Vector3 quadScale = new Vector3(UiHoverMeshRect.SizeInset*2, 0.36f, 0.1f);
				float percPerTick = 1/(float)(vSliderItem.Ticks-1);

				vTickMeshBuilder = new MeshBuilder();
				MeshUtil.BuildQuadMesh(vTickMeshBuilder);
				vTickMeshBuilder.Commit();
				vTickMeshBuilder.CommitColors(Color.clear);

				for ( int i = 0 ; i < vSliderItem.Ticks ; ++i ) {
					GameObject tickObj = new GameObject("Tick"+i);
					tickObj.transform.SetParent(gameObject.transform, false);
					tickObj.transform.localPosition = Vector3.right*(vSlideX0+vSlideW*i*percPerTick);
					tickObj.transform.localRotation = TickQuatRot;
					tickObj.transform.localScale = quadScale;
					tickObj.AddComponent<MeshRenderer>();

					MeshFilter tickFilt = tickObj.AddComponent<MeshFilter>();
					tickFilt.sharedMesh = vTickMeshBuilder.Mesh;

					vTicks[i] = tickObj;
				}
			}

			////

			vGrabHold = new GameObject("GrabHold");
			vGrabHold.transform.SetParent(gameObject.transform, false);
			vGrabHold.transform.localRotation = Quaternion.Inverse(gameObject.transform.localRotation);

			var grabObj = new GameObject("Grab");
			grabObj.transform.SetParent(vGrabHold.transform, false);

			vGrab = grabObj.AddComponent<UiItemSliderGrabRenderer>();
			vGrab.IsVert = vIsVert;
			vGrab.Build(vPanelState, vLayoutState, vItemState, vSettings);

			if ( vIsVert ) {
				vGrab.SetCustomSize(vHeight, vGrabW, false);
			}
			else {
				vGrab.SetCustomSize(vGrabW, vHeight, false);
			}

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
			vHover.SetDepthHint(pDepthHint);
			vGrab.SetDepthHint(pDepthHint);
			vTrack.SetDepthHint(pDepthHint);

			var tickMat = Materials.GetLayer(Materials.Layer.Ticks, pDepthHint);

			foreach ( GameObject tickObj in vTicks ) {
				tickObj.GetComponent<MeshRenderer>().sharedMaterial = tickMat;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = vPanelState.DisplayStrength*vLayoutState.DisplayStrength;

			if ( !vSliderItem.IsEnabled || !vSliderItem.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}

			const int easePower = 3;
			int snaps = vSliderItem.Snaps;
			float easedVal = DisplayUtil.GetEasedValue(
				snaps, vSliderItem.Value, vSliderItem.SnappedValue, easePower);
			float easedHover = easedVal;
			float hoverW = 0;

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
		public virtual void UpdateHoverPoints(IBaseItemPointsState pPointsState, 
																			Vector3 pCursorWorldPos) {
			if ( vSliderItem.AllowJump ) {
				vHiddenRect.UpdateHoverPoints(pPointsState);
				return;
			}

			vGrab.UpdateHoverPoints(pPointsState, pCursorWorldPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshes(float pValue, float pHoverValue, float pHoverW) {
			vTrackSegments.Clear();
			vTrackCuts.Clear();

			float halfGrabW = vGrabW/2;
			float valCenter = pValue*vSlideW + halfGrabW;
			float hovCenter = pHoverValue*vSlideW + halfGrabW;
			float zeroCenter = vZeroValue*vSlideW + halfGrabW;
			bool isHoverTooClose = (Math.Abs(valCenter-hovCenter) < vGrabW*(0.5f+HoverBarRelW));

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
					EndValue = vWidth
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
					EndValue = vWidth,
					IsFill = !isMinStart
				});
			}
			
			vTrackCuts.Add(new DisplayUtil.TrackSegment {
				StartValue = valCenter - halfGrabW,
				EndValue = valCenter + halfGrabW,
			});

			if ( pHoverW > 0 && !isHoverTooClose ) {
				vTrackCuts.Add(new DisplayUtil.TrackSegment {
					StartValue = hovCenter - pHoverW/2,
					EndValue = hovCenter + pHoverW/2,
				});

				vHover.UpdateSize(pHoverW, vHeight);
			}
			else {
				vHover.UpdateSize(0, 0);
			}

			vTrack.UpdateSegments(vTrackSegments.ReadOnly, vTrackCuts.ReadOnly, vHeight*0.92f);
		}

	}

}
