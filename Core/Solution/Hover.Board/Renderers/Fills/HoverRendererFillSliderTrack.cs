using System.Collections.Generic;
using Hover.Board.Renderers.Meshes;
using Hover.Board.Renderers.Utils;
using UnityEngine;

namespace Hover.Board.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererFillSliderTrack : HoverRendererFill {
	
		public List<SliderUtil.Segment> SegmentInfoList { get; set; }
	
		public HoverRendererMeshSliderRectangle[] Segments;
		
		[Range(0, 100)]
		public float SizeX = 10;

		[Range(0, 100)]
		public float InsetL = 1;

		[Range(0, 100)]
		public float InsetR = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int MaterialRenderQueue {
			get {
				return Segments[0].MaterialRenderQueue;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateAfterRenderer() {
			if ( SegmentInfoList != null ) {
				UpdateSegmentsWithInfo();
			}

			foreach ( HoverRendererMeshSliderRectangle seg in Segments ) {
				if ( seg.gameObject.activeSelf ) {
					seg.UpdateAfterRenderer();
				}
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
			
			foreach ( HoverRendererMeshSliderRectangle seg in Segments ) {
				seg.SizeY = 0;
			}
			
			foreach ( SliderUtil.Segment segInfo in SegmentInfoList ) {
				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}
					
				HoverRendererMeshSliderRectangle seg = Segments[segIndex++];
				seg.SizeY = segInfo.EndPosition-segInfo.StartPosition;
				seg.IsFill = segInfo.IsFill;

				seg.transform.localPosition = new Vector3(
					(InsetL-InsetR)/2,
					(segInfo.StartPosition+segInfo.EndPosition)/2,
					0
				);
			}
			
			foreach ( HoverRendererMeshSliderRectangle seg in Segments ) {
				seg.ControlledByRenderer = true;
				seg.SizeX = insetSizeX;
				seg.gameObject.SetActive(seg.SizeY > 0);
			}
		}
		
	}

}
