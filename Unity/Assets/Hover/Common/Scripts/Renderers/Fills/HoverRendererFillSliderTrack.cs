using System.Collections.Generic;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Renderers.Meshes;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererFillSliderTrack : HoverRendererFill, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string AlphaName = "Alpha";

		public List<SliderUtil.Segment> SegmentInfoList { get; set; }
	
		public HoverRendererMeshSliderRectangle[] Segments;
		
		[Range(0, 100)]
		[DisableWhenControlled(DisplayMessage=true)]
		public float SizeX = 10;

		[Range(0, 100)]
		[DisableWhenControlled]
		public float InsetL = 1;

		[Range(0, 100)]
		[DisableWhenControlled]
		public float InsetR = 1;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float Alpha = 1;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int MaterialRenderQueue {
			get {
				return Segments[0].MaterialRenderQueue;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( SegmentInfoList != null ) {
				UpdateSegmentsWithInfo();
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Segments = new HoverRendererMeshSliderRectangle[4];
			
			for ( int i = 0 ; i < Segments.Length ; i++ ) {
				HoverRendererMeshSliderRectangle seg = BuildRectangle("Segment"+i);
				seg.TrackColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
				seg.FillColor = new Color(0.1f, 0.9f, 0.2f);
				Segments[i] = seg;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererMeshSliderRectangle BuildRectangle(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererMeshSliderRectangle>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSegmentsWithInfo() {
			int segIndex = 0;
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);
			float trackStartY = SegmentInfoList[0].StartPosition;
			float trackEndY = SegmentInfoList[SegmentInfoList.Count-1].EndPosition;
			
			foreach ( HoverRendererMeshSliderRectangle seg in Segments ) {
				seg.Controllers.Set("GameObject.activeSelf", this);
				seg.Controllers.Set("Transform.localPosition", this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.SizeXName, this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.SizeYName, this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.AlphaName, this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.UvStartYName, this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.UvEndYName, this);
				seg.Controllers.Set(HoverRendererMeshSliderRectangle.IsFillName, this);

				seg.SizeY = 0;
				seg.Alpha = Alpha;
			}
			
			foreach ( SliderUtil.Segment segInfo in SegmentInfoList ) {
				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				HoverRendererMeshSliderRectangle seg = Segments[segIndex++];
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
			
			foreach ( HoverRendererMeshSliderRectangle seg in Segments ) {
				seg.SizeX = insetSizeX;
				RendererHelper.SetActiveWithUpdate(seg, (seg.SizeY > 0));
			}
		}
		
	}

}
