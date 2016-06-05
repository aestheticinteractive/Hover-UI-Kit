using System.Collections.Generic;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public abstract class HoverFillArcSlider : HoverFill {
	
		public const string SegmentInfoListName = "SegmentInfoList";
		public const string TickInfoListName = "TickInfoList";
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const int SegmentCount = 4;

		//TODO: DisableWhenControlled
		public List<SliderUtil.SegmentInfo> SegmentInfoList;
		public List<SliderUtil.SegmentInfo> TickInfoList;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float OuterRadius = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InnerRadius = 4;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetOuter = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetInner = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TickRelativeSizeX = 0.5f;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			if ( SegmentInfoList != null ) {
				UpdateSegmentsWithInfo();
			}

			if ( TickInfoList != null ) {
				UpdateTickCount(TickInfoList.Count);
				UpdateTicksWithInfo();
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract HoverMeshArcTrack GetSegment(int pIndex);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract HoverMeshArcTrack GetTick(int pIndex);
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateTickCount(int pCount);
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSegmentsWithInfo() {
			int segIndex = 0;
			float trackStartY = SegmentInfoList[0].StartPosition;
			float trackEndY = SegmentInfoList[SegmentInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshArcTrack seg = GetSegment(i);
				
				seg.Controllers.Set("GameObject.activeSelf", this);
				seg.Controllers.Set("Transform.localRotation", this);
				seg.Controllers.Set(HoverMeshArc.OuterRadiusName, this);
				seg.Controllers.Set(HoverMeshArc.InnerRadiusName, this);
				seg.Controllers.Set(HoverMeshArc.ArcAngleName, this);
				seg.Controllers.Set(HoverMeshArcTrack.UvStartYName, this);
				seg.Controllers.Set(HoverMeshArcTrack.UvEndYName, this);
				seg.Controllers.Set(HoverMeshArcTrack.IsFillName, this);
				seg.Controllers.Set(HoverMesh.SortingLayerName, this);

				seg.ArcAngle = 0;
				seg.OuterRadius = OuterRadius-InsetOuter;
				seg.InnerRadius = InnerRadius+InsetInner;
				seg.SortingLayer = SortingLayer;
			}

			for ( int i = 0 ; i < SegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = SegmentInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				HoverMeshArcTrack seg = GetSegment(segIndex++);
				seg.ArcAngle = segInfo.EndPosition-segInfo.StartPosition;
				seg.IsFill = segInfo.IsFill;
				seg.UvStartY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.StartPosition) : 0);
				seg.UvEndY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.EndPosition) : 1);

				seg.transform.localRotation = Quaternion.AngleAxis(
					(segInfo.StartPosition+segInfo.EndPosition)/2, Vector3.forward);
			}

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshArcTrack seg = GetSegment(i);
				RendererUtil.SetActiveWithUpdate(seg, (seg.ArcAngle != 0));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateTicksWithInfo() {
			float tickInset = (OuterRadius-InnerRadius-InsetOuter-InsetInner)*(1-TickRelativeSizeX)/2;

			for ( int i = 0 ; i < TickInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo tickInfo = TickInfoList[i];
				HoverMeshArcTrack tick = GetTick(i);

				tick.Controllers.Set("GameObject.activeSelf", this);
				tick.Controllers.Set("Transform.localRotation", this);
				tick.Controllers.Set(HoverMeshArc.OuterRadiusName, this);
				tick.Controllers.Set(HoverMeshArc.InnerRadiusName, this);
				tick.Controllers.Set(HoverMeshArc.ArcAngleName, this);
				tick.Controllers.Set(HoverMesh.SortingLayerName, this);

				RendererUtil.SetActiveWithUpdate(tick, !tickInfo.IsHidden);

				tick.OuterRadius = OuterRadius-InsetOuter-tickInset;
				tick.InnerRadius = InnerRadius+InsetInner+tickInset;
				tick.ArcAngle = tickInfo.EndPosition-tickInfo.StartPosition;
				tick.SortingLayer = SortingLayer;

				tick.transform.localRotation = Quaternion.AngleAxis(
					(tickInfo.StartPosition+tickInfo.EndPosition)/2, Vector3.forward);
			}
		}
		
	}

}
