using System.Collections.Generic;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Board.Renderers.Utils {

	/*================================================================================================*/
	public static class SliderUtil {
	
		public enum SegmentType {
			Track,
			Handle,
			Jump,
			Zero
		}
		
		public enum PositionType {
			TrackStart,
			TrackEnd,
			HandleStart,
			HandleEnd,
			JumpStart,
			JumpEnd,
			Zero
		}
	
		public struct Segment {
			public SegmentType Type;
			public PositionType StartPositionType;
			public PositionType EndPositionType;
			public float StartPosition;
			public float EndPosition;
			public bool IsFill;
			public bool? IsZeroAtStart;
		}
		
		public struct SliderInfo {
			public SliderItem.FillType FillType;
			public float TrackStartPosition;
			public float TrackEndPosition;
			public float HandleSize;
			public float HandlePosition;
			public float JumpSize;
			public float JumpPosition;
			public float ZeroPosition;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void CalculateSegments(SliderInfo pInfo, List<Segment> pSegments) {
			pSegments.Clear();

			int posMult = (pInfo.TrackStartPosition < pInfo.TrackEndPosition ? 1 : -1);
			float half = 0.5f*posMult;
			bool hasJump = (pInfo.JumpSize > 0);
			bool isJumpTooNear = (Mathf.Abs(pInfo.HandlePosition-pInfo.JumpPosition) <
				pInfo.HandleSize+pInfo.JumpSize);
			
			var handleSeg = new Segment {
				Type = SegmentType.Handle,
				StartPositionType = PositionType.HandleStart,
				EndPositionType = PositionType.HandleEnd,
				StartPosition = pInfo.HandlePosition-pInfo.HandleSize*half,
				EndPosition = pInfo.HandlePosition+pInfo.HandleSize*half
			};
			
			pSegments.Add(handleSeg);
			
			if ( hasJump && !isJumpTooNear ) {
				var jumpSeg = new Segment {
					Type = SegmentType.Jump,
					StartPositionType = PositionType.JumpStart,
					EndPositionType = PositionType.JumpEnd,
					StartPosition = pInfo.HandlePosition-pInfo.HandleSize*half,
					EndPosition = pInfo.HandlePosition+pInfo.HandleSize*half
				};
				
				pSegments.Insert((pInfo.HandlePosition < pInfo.JumpPosition ? 1 : 0), jumpSeg);
			}

			if ( pInfo.FillType == SliderItem.FillType.Zero ) {
				var zeroSeg = new Segment {
					Type = SegmentType.Zero,
					StartPositionType = PositionType.Zero,
					EndPositionType = PositionType.Zero,
					StartPosition = pInfo.ZeroPosition,
					EndPosition = pInfo.ZeroPosition
				};
				
				if ( pInfo.ZeroPosition*posMult < pSegments[0].StartPosition*posMult ) {
					pSegments.Insert(0, zeroSeg);
				}
				else if ( pSegments.Count > 1 && 
						pInfo.ZeroPosition*posMult < pSegments[1].StartPosition*posMult ) {
					pSegments.Insert(1, zeroSeg);
				}
				else {
					pSegments.Add(zeroSeg);
				}
			}
			
			//TODO: add track segments between the handle/jump/zero segments
		}

	}

}
