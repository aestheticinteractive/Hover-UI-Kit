using System;
using System.Linq;
using Hover.Common.Items;
using Hover.Common.State;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public static class DisplayUtil {
		
		public struct TrackSegment {
			public float StartValue;
			public float EndValue;
			public bool IsFill;
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
		public static TrackSegment[] SplitTrackSegments(TrackSegment[] pSegments, TrackSegment[] pCuts){
			var slices = pSegments.ToList();

			foreach ( TrackSegment cut in pCuts ) {
				for ( int i = 0 ; i < slices.Count ; i++ ) {
					TrackSegment slice = slices[i];

					if ( cut.StartValue >= slice.StartValue && cut.EndValue <= slice.EndValue ) {
						var slice2 = new TrackSegment();
						slice2.StartValue = cut.EndValue;
						slice2.EndValue = slice.EndValue;
						slice2.IsFill = slice.IsFill;
						slices.Insert(i+1, slice2);

						slice.EndValue = cut.StartValue;
						slices[i] = slice;
					}
					else if ( cut.StartValue >= slice.StartValue && cut.StartValue <= slice.EndValue ) {
						slice.EndValue = cut.StartValue;
						slices[i] = slice;
					}
					else if ( cut.EndValue <= slice.EndValue && cut.EndValue >= slice.StartValue ) {
						slice.StartValue = cut.EndValue;
						slices[i] = slice;
					}
				}
			}

			for ( int i = 0 ; i < slices.Count ; i++ ) {
				TrackSegment slice = slices[i];

				if ( Math.Abs(slice.StartValue-slice.EndValue) <= 0.01f ) {
					slices.RemoveAt(i);
					i--;
				}
			}

			return slices.ToArray();
		}

	}

}
