using System;
using Hover.Common.Items;
using Hover.Common.State;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public static class DisplayUtil {


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

	}

}
