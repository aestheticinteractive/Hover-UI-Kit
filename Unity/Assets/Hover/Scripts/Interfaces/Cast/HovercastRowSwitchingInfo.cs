using Hover.Items;
using Hover.Layouts.Arc;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(SelectableItemData))]
	public class HovercastRowSwitchingInfo : MonoBehaviour {

		public enum RowEntryType {
			Immediate,
			FromOutside,
			FromInside
		}

		public bool NavigateBack = false;
		public HoverLayoutArcRow NavigateToRow = null;
		public RowEntryType RowEntryTransition = RowEntryType.Immediate;

	}

}
