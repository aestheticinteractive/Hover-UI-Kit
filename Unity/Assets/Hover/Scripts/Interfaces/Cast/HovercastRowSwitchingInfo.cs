using Hover.Items;
using Hover.Layouts.Arc;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(SelectableItem))]
	public class HovercastRowSwitchingInfo : MonoBehaviour {
		
		public enum RowEntryType {
			Immediate,
			FromOutside,
			FromInside
		}

		public HoverLayoutArcRow TargetRow;
		public bool UsePreviousActiveRow = false;
		public RowEntryType RowEntryTransition = RowEntryType.Immediate;

	}

}
