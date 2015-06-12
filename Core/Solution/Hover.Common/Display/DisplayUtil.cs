using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hover.Common.Items;
using Hover.Common.State;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public static class DisplayUtil {
		
		public struct TrackSegment {
			public float StartValue;
			public float EndValue;
			public bool IsFill;
			public bool? IsZeroAtStart;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetEasedValue(int pSnaps, float pValue, float pSnappedValue, float pPower) {
			if ( pSnaps < 2 ) {
				return pValue;
			}

			float showVal = pSnappedValue;
			int snaps = pSnaps-1;
			float diff = pValue-showVal;
			int sign = Math.Sign(diff);

			diff = Math.Abs(diff); //between 0 and 1
			diff *= snaps;

			if ( diff < 0.5 ) {
				diff *= 2;
				diff = (float)Math.Pow(diff, pPower);
				diff /= 2f;
			}
			else {
				diff = (diff-0.5f)*2;
				diff = 1-(float)Math.Pow(1-diff, pPower);
				diff = diff/2f+0.5f;
			}

			diff /= snaps;
			return showVal + diff*sign;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static bool IsEdgeVisible(IBaseItemState pItemState) {
			ISelectableItem selItem = (pItemState.Item as ISelectableItem);

			if ( selItem == null || !pItemState.IsNearestHighlight || !selItem.AllowSelection ) {
				return false;
			}

			return (!pItemState.IsSelectionPrevented || selItem.IsStickySelected);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void SplitTrackSegments(ReadOnlyCollection<TrackSegment> pSegments, 
							ReadOnlyCollection<TrackSegment> pCuts, IList<TrackSegment> pSliceResults) {
			pSliceResults.Clear();

			for ( int segI = 0 ; segI < pSegments.Count ; segI++ ) {
				TrackSegment seg = pSegments[segI];
				pSliceResults.Add(seg);
			}

			for ( int cutI = 0 ; cutI < pCuts.Count ; cutI++ ) {
				TrackSegment cut = pCuts[cutI];

				for ( int sliceI = 0 ; sliceI < pSliceResults.Count ; sliceI++ ) {
					TrackSegment slice = pSliceResults[sliceI];

					if ( cut.StartValue >= slice.StartValue && cut.EndValue <= slice.EndValue ) {
						var slice2 = new TrackSegment();
						slice2.StartValue = cut.EndValue;
						slice2.EndValue = slice.EndValue;
						slice2.IsFill = slice.IsFill;
						pSliceResults.Insert(sliceI+1, slice2);

						slice.EndValue = cut.StartValue;
						pSliceResults[sliceI] = slice;
						continue;
					}

					if ( cut.StartValue >= slice.StartValue && cut.StartValue <= slice.EndValue ) {
						slice.EndValue = cut.StartValue;
						pSliceResults[sliceI] = slice;
						continue;
					}

					if ( cut.EndValue <= slice.EndValue && cut.EndValue >= slice.StartValue ) {
						slice.StartValue = cut.EndValue;
						pSliceResults[sliceI] = slice;
						continue;
					}

					if ( cut.StartValue <= slice.StartValue && cut.EndValue >= slice.EndValue ) {
						pSliceResults.RemoveAt(sliceI);
						sliceI--;
					}
				}
			}

			for ( int sliceI = 0 ; sliceI < pSliceResults.Count ; sliceI++ ) {
				TrackSegment slice = pSliceResults[sliceI];

				if ( Math.Abs(slice.StartValue-slice.EndValue) <= 0.01f ) {
					pSliceResults.RemoveAt(sliceI);
					sliceI--;
				}
			}
		}

	}

}
