using System;
using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Utils {

	/*================================================================================================*/
	public static class SliderUtil {
	
		public enum SegmentType {
			Track,
			Handle,
			Jump,
			Start,
			Zero,
			End,
			Tick
		}
		
		public enum PositionType {
			TrackStart,
			TrackEnd,
			HandleStart,
			HandleEnd,
			JumpStart,
			JumpEnd,
			Zero,
			TickStart,
			TickEnd
		}

		[Serializable]
		public struct SegmentInfo {
			public SegmentType Type;
			public PositionType StartPositionType;
			public PositionType EndPositionType;
			public float StartPosition;
			public float EndPosition;
			public bool IsFill;
			public bool IsHidden;
		}

		[Serializable]
		public struct SliderInfo {
			public SliderFillType FillType;
			public float TrackStartPosition;
			public float TrackEndPosition;
			public float HandleSize;
			public float HandleValue;
			public float JumpSize;
			public float JumpValue;
			public float ZeroValue;
			public int TickCount;
			public float TickSize;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void CalculateSegments(SliderInfo pInfo, List<SegmentInfo> pSegments) {
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
			
			var handleSeg = new SegmentInfo {
				Type = SegmentType.Handle,
				StartPositionType = PositionType.HandleStart,
				EndPositionType = PositionType.HandleEnd,
				StartPosition = handlePos-pInfo.HandleSize*half,
				EndPosition = handlePos+pInfo.HandleSize*half
			};
			
			pSegments.Add(handleSeg);
			
			if ( hasJump && !isJumpTooNear && pInfo.JumpValue >= 0 ) {
				var jumpSeg = new SegmentInfo {
					Type = SegmentType.Jump,
					StartPositionType = PositionType.JumpStart,
					EndPositionType = PositionType.JumpEnd,
					StartPosition = jumpPos-pInfo.JumpSize*half,
					EndPosition = jumpPos+pInfo.JumpSize*half
				};
				
				pSegments.Insert((handlePos*mult < jumpPos*mult ? 1 : 0), jumpSeg);
			}

			////

			if ( pInfo.FillType == SliderFillType.Zero ) {
				var zeroSeg = new SegmentInfo {
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
					SegmentInfo beforeZeroSeg = pSegments[zeroI-1];
					
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

			var startSeg = new SegmentInfo {
				Type = SegmentType.Start,
				StartPositionType = PositionType.TrackStart,
				EndPositionType = PositionType.TrackStart,
				StartPosition = pInfo.TrackStartPosition,
				EndPosition = pInfo.TrackStartPosition
			};
			
			var endSeg = new SegmentInfo {
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
				case SliderFillType.Zero:
					fillToSegType = SegmentType.Zero;
					break;

				case SliderFillType.MinimumValue:
					fillToSegType = SegmentType.Start;
					break;
			
				case SliderFillType.MaximumValue:
					fillToSegType = SegmentType.End;
					break;

				default:
					throw new Exception("Unhandled fill type: "+pInfo.FillType);
			}

			////

			for ( int i = 1 ; i < pSegments.Count ; i++ ) {
				SegmentInfo prevSeg = pSegments[i-1];
				SegmentInfo nextSeg = pSegments[i];

				if ( prevSeg.Type == SegmentType.Handle || prevSeg.Type == fillToSegType ) {
					isFilling = !isFilling;
				}

				var trackSeg = new SegmentInfo {
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

		/*--------------------------------------------------------------------------------------------*/
		public static void CalculateTicks(SliderInfo pInfo, List<SegmentInfo> pSliderSegments,
																			List<SegmentInfo> pTicks) {
			pTicks.Clear();

			if ( pInfo.TickCount <= 1 ) {
				return;
			}
			
			////

			float handStart = -1;
			float handEnd = -1;
			float jumpStart = -1;
			float jumpEnd = -1;

			for ( int i = 0 ; i < pSliderSegments.Count ; i++ ) {
				SegmentInfo seg = pSliderSegments[i];

				if ( seg.Type == SegmentType.Handle ) {
					handStart = seg.StartPosition;
					handEnd = seg.EndPosition;
				}

				if ( seg.Type == SegmentType.Jump ) {
					jumpStart = seg.StartPosition;
					jumpEnd = seg.EndPosition;
				}
			}

			////

			int mult = (pInfo.TrackStartPosition < pInfo.TrackEndPosition ? 1 : -1);
			float half = 0.5f*mult;
			float handleMinPos = pInfo.TrackStartPosition + pInfo.HandleSize*half;
			float handleMaxPos = pInfo.TrackEndPosition - pInfo.HandleSize*half;
			
			for ( int i = 0 ; i < pInfo.TickCount ; i++ ) {
				float prog = i/(pInfo.TickCount-1f);
				float tickCenterPos = Mathf.Lerp(handleMinPos, handleMaxPos, prog);

				var tick = new SegmentInfo {
					Type = SegmentType.Tick,
					StartPositionType = PositionType.TickStart,
					EndPositionType = PositionType.TickEnd,
					StartPosition = tickCenterPos-pInfo.TickSize*half,
					EndPosition = tickCenterPos+pInfo.TickSize*half
				};

				float startMult = tick.StartPosition*mult;
				float endMult = tick.EndPosition*mult;
				bool startsInHand = (startMult >= handStart*mult && startMult <= handEnd*mult);
				bool endsInHand   = (endMult   >= handStart*mult && endMult   <= handEnd*mult);
				bool startsInJump = (startMult >= jumpStart*mult && startMult <= jumpEnd*mult);
				bool endsInJump   = (endMult   >= jumpStart*mult && endMult   <= jumpEnd*mult);

				tick.IsHidden = ((startsInHand && endsInHand) || (startsInJump && endsInJump));

				if ( startsInHand ) {
					tick.StartPosition = handEnd;
				}

				if ( endsInHand ) {
					tick.EndPosition = handStart;
				}

				if ( startsInJump ) {
					tick.StartPosition = jumpEnd;
				}

				if ( endsInJump ) {
					tick.EndPosition = jumpStart;
				}

				pTicks.Add(tick);
			}
		}

	}

}
