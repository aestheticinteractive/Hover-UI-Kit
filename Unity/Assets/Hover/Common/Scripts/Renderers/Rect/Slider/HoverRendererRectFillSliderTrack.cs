using System.Collections.Generic;
using Hover.Common.Renderers.Shared.Bases;
using Hover.Common.Renderers.Shared.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Rect.Slider {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectFillSliderTrack : HoverRendererFill, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string AlphaName = "Alpha";

		public List<SliderUtil.Segment> SegmentInfoList { get; set; }
	
		public HoverRendererRectMeshForSliderTrack[] Segments;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetL = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InsetR = 1;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			if ( SegmentInfoList != null ) {
				UpdateSegmentsWithInfo();
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Segments = new HoverRendererRectMeshForSliderTrack[4];
			
			for ( int i = 0 ; i < Segments.Length ; i++ ) {
				HoverRendererRectMeshForSliderTrack seg = BuildRectangle("Segment"+i);
				seg.TrackColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
				seg.FillColor = new Color(0.1f, 0.9f, 0.2f);
				Segments[i] = seg;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererRectMeshForSliderTrack BuildRectangle(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererRectMeshForSliderTrack>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSegmentsWithInfo() {
			int segIndex = 0;
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);
			float trackStartY = SegmentInfoList[0].StartPosition;
			float trackEndY = SegmentInfoList[SegmentInfoList.Count-1].EndPosition;
			
			foreach ( HoverRendererRectMeshForSliderTrack seg in Segments ) {
				seg.Controllers.Set("GameObject.activeSelf", this);
				seg.Controllers.Set("Transform.localPosition", this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.SizeXName, this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.SizeYName, this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.AlphaName, this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.UvStartYName, this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.UvEndYName, this);
				seg.Controllers.Set(HoverRendererRectMeshForSliderTrack.IsFillName, this);
				seg.Controllers.Set(HoverRendererMesh.SortingLayerName, this);

				seg.SizeY = 0;
				seg.Alpha = Alpha;
				seg.SortingLayer = SortingLayer;
			}
			
			foreach ( SliderUtil.Segment segInfo in SegmentInfoList ) {
				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				HoverRendererRectMeshForSliderTrack seg = Segments[segIndex++];
				seg.SizeY = segInfo.EndPosition-segInfo.StartPosition;
				seg.IsFill = segInfo.IsFill;
				seg.UvStartY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.StartPosition) : 0);
				seg.UvEndY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.EndPosition) : 1);

				seg.transform.localPosition = new Vector3(
					(InsetL-InsetR)/2,
					(segInfo.StartPosition+segInfo.EndPosition)/2,
					0
				);
			}
			
			foreach ( HoverRendererRectMeshForSliderTrack seg in Segments ) {
				seg.SizeX = insetSizeX;
				RendererHelper.SetActiveWithUpdate(seg, (seg.SizeY > 0));
			}
		}
		
	}

}
