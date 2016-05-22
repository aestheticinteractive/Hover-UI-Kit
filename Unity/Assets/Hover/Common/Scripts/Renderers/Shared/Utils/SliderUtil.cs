using System;
using System.Collections.Generic;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Renderers.Shared.Utils {

	/*================================================================================================*/
	public static class SliderUtil {
	
		public enum SegmentType {
			Track,
			Handle,
			Jump,
			Start,
			Zero,
			End
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
		}
		
		public struct SliderInfo {
			public SliderItem.FillType FillType;
			public float TrackStartPosition;
			public float TrackEndPosition;
			public float HandleSize;
			public float HandleValue;
			public float JumpSize;
			public float JumpValue;
			public float ZeroValue;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void CalculateSegments(SliderInfo pInfo, List<Segment> pSegments) {
			pSegments.Clear();

			int mult = (pInfo.TrackStartPosition < pInfo.TrackEndPosition ? 1 : -1);
			float half = 0.5f*mult;
			float handleMinPos = pInfo.TrackStartPosition + pInfo.HandleSize*half;
			float handleMaxPos = pInfo.TrackEndPosition - pInfo.HandleSize*half;
			float handlePos = Mathf.Lerp(handleMinPos, handleMaxPos, pInfo.HandleValue);
			float jumpPos = Mathf.Lerp(handleMinPos, handleMaxPos, pInfo.JumpValue);
			float zeroPos = Mathf.Lerp(handleMinPos, handleMaxPos, pInfo.ZeroValue);
			bool hasJump = (pInfo.JumpSize > 0);
			bool isJumpTooNear = (Mathf.Abs(handlePos-jumpPos) < 
				(pInfo.HandleSize+pInfo.JumpSize)*0.6f);
			
			var handleSeg = new Segment {
				Type = SegmentType.Handle,
				StartPositionType = PositionType.HandleStart,
				EndPositionType = PositionType.HandleEnd,
				StartPosition = handlePos-pInfo.HandleSize*half,
				EndPosition = handlePos+pInfo.HandleSize*half
			};
			
			pSegments.Add(handleSeg);
			
			if ( hasJump && !isJumpTooNear && pInfo.JumpValue >= 0 ) {
				var jumpSeg = new Segment {
					Type = SegmentType.Jump,
					StartPositionType = PositionType.JumpStart,
					EndPositionType = PositionType.JumpEnd,
					StartPosition = jumpPos-pInfo.JumpSize*half,
					EndPosition = jumpPos+pInfo.JumpSize*half
				};
				
				pSegments.Insert((handlePos*mult < jumpPos*mult ? 1 : 0), jumpSeg);
			}

			////

			if ( pInfo.FillType == SliderItem.FillType.Zero ) {
				var zeroSeg = new Segment {
					Type = SegmentType.Zero,
					StartPositionType = PositionType.Zero,
					EndPositionType = PositionType.Zero,
					StartPosition = zeroPos,
					EndPosition = zeroPos
				};
				
				int zeroI;
				
				if ( zeroPos*mult < pSegments[0].StartPosition*mult ) {
					zeroI = 0;
					pSegments.Insert(zeroI, zeroSeg);
				}
				else if ( pSegments.Count > 1 && zeroPos*mult < pSegments[1].StartPosition*mult ) {
					zeroI = 1;
					pSegments.Insert(zeroI, zeroSeg);
				}
				else {
					zeroI = pSegments.Count;
					pSegments.Add(zeroSeg);
				}
								
				if ( zeroI > 0 ) {
					Segment beforeZeroSeg = pSegments[zeroI-1];
					
					if ( zeroSeg.StartPosition*mult < beforeZeroSeg.EndPosition*mult ) {
						zeroSeg.StartPosition = beforeZeroSeg.EndPosition;
						zeroSeg.EndPosition = beforeZeroSeg.EndPosition;
						zeroSeg.StartPositionType = beforeZeroSeg.EndPositionType;
						zeroSeg.EndPositionType = beforeZeroSeg.EndPositionType;
						pSegments[zeroI] = zeroSeg;
					}
				}
			}
			
			////

			var startSeg = new Segment {
				Type = SegmentType.Start,
				StartPositionType = PositionType.TrackStart,
				EndPositionType = PositionType.TrackStart,
				StartPosition = pInfo.TrackStartPosition,
				EndPosition = pInfo.TrackStartPosition
			};
			
			var endSeg = new Segment {
				Type = SegmentType.End,
				StartPositionType = PositionType.TrackEnd,
				EndPositionType = PositionType.TrackEnd,
				StartPosition = pInfo.TrackEndPosition,
				EndPosition = pInfo.TrackEndPosition
			};
			
			pSegments.Insert(0, startSeg);
			pSegments.Add(endSeg);

			////

			bool isFilling = false;
			SegmentType fillToSegType;

			switch ( pInfo.FillType ) {
				case SliderItem.FillType.Zero:
					fillToSegType = SegmentType.Zero;
					break;

				case SliderItem.FillType.MinimumValue:
					fillToSegType = SegmentType.Start;
					break;
			
				case SliderItem.FillType.MaximumValue:
					fillToSegType = SegmentType.End;
					break;

				default:
					throw new Exception("Unhandled fill type: "+pInfo.FillType);
			}

			////

			for ( int i = 1 ; i < pSegments.Count ; i++ ) {
				Segment prevSeg = pSegments[i-1];
				Segment nextSeg = pSegments[i];

				if ( prevSeg.Type == SegmentType.Handle || prevSeg.Type == fillToSegType ) {
					isFilling = !isFilling;
				}

				var trackSeg = new Segment {
					Type = SegmentType.Track,
					StartPositionType = prevSeg.EndPositionType,
					EndPositionType = nextSeg.StartPositionType,
					StartPosition = prevSeg.EndPosition,
					EndPosition = nextSeg.StartPosition,
					IsFill = isFilling
				};

				pSegments.Insert(i, trackSeg);
				i++;
			}
		}

	}

}
