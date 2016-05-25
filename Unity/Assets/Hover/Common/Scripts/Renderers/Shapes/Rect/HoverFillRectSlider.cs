using System.Collections.Generic;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverFillRectSlider : HoverFill {
	
		public const string SizeXName = "SizeX";
		public const int SegmentCount = 4;

		public List<SliderUtil.SegmentInfo> SegmentInfoList { get; set; }
		public List<SliderUtil.SegmentInfo> TickInfoList { get; set; }
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetL = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetR = 1;
		
		[DisableWhenControlled(RangeMin=0.01f, RangeMax=1)]
		public float TickSizeY = 0.06f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TickRelativeSizeX = 0.5f;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
		protected abstract HoverMeshRectTrack GetSegment(int pIndex);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract HoverMeshRectTrack GetTick(int pIndex);
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateTickCount(int pCount);
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSegmentsWithInfo() {
			int segIndex = 0;
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);
			float trackStartY = SegmentInfoList[0].StartPosition;
			float trackEndY = SegmentInfoList[SegmentInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshRectTrack seg = GetSegment(i);
				
				seg.Controllers.Set("GameObject.activeSelf", this);
				seg.Controllers.Set("Transform.localPosition.x", this);
				seg.Controllers.Set("Transform.localPosition.y", this);
				seg.Controllers.Set(HoverMeshRectTrack.SizeXName, this);
				seg.Controllers.Set(HoverMeshRectTrack.SizeYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.UvStartYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.UvEndYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.IsFillName, this);
				seg.Controllers.Set(HoverMesh.SortingLayerName, this);

				seg.SizeY = 0;
				seg.SizeX = insetSizeX;
				seg.SortingLayer = SortingLayer;
			}

			for ( int i = 0 ; i < SegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = SegmentInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				HoverMeshRectTrack seg = GetSegment(segIndex++);
				seg.SizeY = segInfo.EndPosition-segInfo.StartPosition;
				seg.IsFill = segInfo.IsFill;
				seg.UvStartY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.StartPosition) : 0);
				seg.UvEndY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.EndPosition) : 1);

				Vector3 localPos = seg.transform.localPosition;
				localPos.x = (InsetL-InsetR)/2;
				localPos.y = (segInfo.StartPosition+segInfo.EndPosition)/2;
				seg.transform.localPosition = localPos;
			}

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshRectTrack seg = GetSegment(i);
				RendererUtil.SetActiveWithUpdate(seg, (seg.SizeY != 0));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateTicksWithInfo() {
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);

			for ( int i = 0 ; i < TickInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo tickInfo = TickInfoList[i];
				HoverMeshRectTrack tick = GetTick(i);

				tick.Controllers.Set("Transform.localPosition.x", this);
				tick.Controllers.Set("Transform.localPosition.y", this);
				tick.Controllers.Set(HoverMeshRectTrack.SizeXName, this);
				tick.Controllers.Set(HoverMeshRectTrack.SizeYName, this);
				tick.Controllers.Set(HoverMesh.SortingLayerName, this);

				tick.SizeX = insetSizeX*TickRelativeSizeX;
				tick.SizeY = tickInfo.EndPosition-tickInfo.StartPosition;
				tick.SortingLayer = SortingLayer;

				Vector3 localPos = tick.transform.localPosition;
				localPos.x = (InsetL-InsetR)/2;
				localPos.y = (tickInfo.StartPosition+tickInfo.EndPosition)/2;
				tick.transform.localPosition = localPos;
			}
		}
		
	}

}
